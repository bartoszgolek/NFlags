using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NFlags.Arguments;
using NFlags.TypeConverters;
using NFlags.Utils;
using NFlags.ValueProviders;

namespace NFlags.Commands
{
    internal class CommandExecutionContextProvider
    {
        private readonly CommandConfig _commandConfig;
        private readonly Shifter<Parameter> _parameters;
        private readonly ParameterSeries _parameterSeries;
        private readonly Shifter<string> _args;
        private readonly CommandArgs _commandArgs;
        private readonly IDictionary<string, ArrayAggregator> _optionValues;

        public CommandExecutionContextProvider(
            CommandConfig commandConfig,
            string[] args
        )
        {
            _commandConfig = commandConfig;

            InitBaseOptions();

            _parameters = new Shifter<Parameter>(commandConfig.Parameters.ToArray());
            _parameterSeries = commandConfig.ParameterSeries;
            _args = new Shifter<string>(args);
            _commandArgs = InitDefaultCommandArgs();
            _optionValues = new Dictionary<string, ArrayAggregator>();
        }

        private void InitBaseOptions()
        {
            _commandConfig.Options.Add(
                new Flag
                {
                    Name = _commandConfig.CliConfig.HelpConfig.Flag,
                    Abr = _commandConfig.CliConfig.HelpConfig.Abr,
                    Description = _commandConfig.CliConfig.HelpConfig.Description,
                    DefaultValue = false
                }
            );

            if (_commandConfig.CliConfig.VersionConfig.Enabled)
            {
                _commandConfig.Options.Add(
                    new Flag
                    {
                        Name = _commandConfig.CliConfig.VersionConfig.Flag,
                        Abr = _commandConfig.CliConfig.VersionConfig.Abr,
                        Description = _commandConfig.CliConfig.VersionConfig.Description,
                        DefaultValue = false
                    }
                );
            }
        }

        public CommandExecutionContext GetFromArgs()
        {
            try
            {
                return PrepareCommandExecutionContext();
            }
            catch (ArgumentValueException e)
            {
                if (!_commandConfig.CliConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(_commandConfig, e.Message);
            }
            catch (TooManyParametersException e)
            {
                if (!_commandConfig.CliConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(_commandConfig, e.Message);
            }
        }

        private CommandExecutionContext PrepareCommandExecutionContext()
        {
            if (CurrentArgIsCommand())
                return GetSubCommand();

            if (HasDefaultCommand())
                return GetDefaultCommand();

            ReadArgsAndOptions();

            if (_commandConfig.CliConfig.VersionConfig.Enabled && _commandArgs.GetFlag(_commandConfig.CliConfig.VersionConfig.Flag))
                return PreparePrintVersionCommandExecutionContext(_commandConfig);

            if (_commandConfig.PrintHelpOnExecute || _commandArgs.GetFlag(_commandConfig.CliConfig.HelpConfig.Flag))
                return PrepareHelpCommandExecutionContext(_commandConfig);

            return new CommandExecutionContext(_commandConfig.Execute, _commandArgs);
        }

        private CommandExecutionContext GetSubCommand()
        {
            var commandName = _args.Current();
            _args.Next();

            return new CommandExecutionContextProvider(
                _commandConfig
                    .Commands
                    .FirstOrDefault(command => command.Name == commandName)
                    ?.GetCommandConfig(_commandConfig),
                _args.ToArray()
            ).GetFromArgs();
        }

        private void ReadArgsAndOptions()
        {
            while (_args.HasData())
            {
                var arg = _args.Shift();

                if (!ReadOpt(arg))
                    ReadParam(arg);
            }

            foreach (var optionValue in _optionValues)
            {
                _commandArgs.AddOptionValueProvider(
                    optionValue.Key,
                    new ConstValueProvider(
                        optionValue.Value.GetArray(
                            _commandConfig.Options.Single(o => o.Name == optionValue.Key).ValueType)
                    )
                );
            }
        }

        private static PrintHelpCommandExecutionContext PrepareHelpCommandExecutionContext(CommandConfig commandConfig, string additionalPrefixMessage)
        {
            return new PrintHelpCommandExecutionContext(additionalPrefixMessage, commandConfig);
        }


        private static PrintHelpCommandExecutionContext PrepareHelpCommandExecutionContext(CommandConfig commandConfig)
        {
            return new PrintHelpCommandExecutionContext(commandConfig);
        }


        private static PrintVersionCommandExecutionContext PreparePrintVersionCommandExecutionContext(CommandConfig commandConfig)
        {
            return new PrintVersionCommandExecutionContext(commandConfig);
        }

        private CommandArgs InitDefaultCommandArgs()
        {
            var commandArgs = new CommandArgs();
            foreach (var option in _commandConfig.Options)
            {
                var valueProviders = GetDefaultValueProvidersInPrecedence(option);
                foreach (var valueProvider in valueProviders)
                    commandArgs.AddOptionValueProvider(option.Name, valueProvider);
            }

            foreach (var parameter in _parameters.ToArray())
            {
                var valueProviders = GetDefaultValueProvidersInPrecedence(parameter);
                foreach (var valueProvider in valueProviders)
                    commandArgs.AddParameterValueProvider(parameter.Name, valueProvider);
            }

            return commandArgs;
        }

        private IEnumerable<IValueProvider> GetDefaultValueProvidersInPrecedence(DefaultValueArgument argument)
        {
            var valueProvidersCollection = new List<IValueProvider>
            {
                new ConstValueProvider(argument.DefaultValue)
            };

            if (argument.ConfigPath != null)
            {
                if (_commandConfig.CliConfig.GenericConfig != null)
                {
                    var valueProvider = argument.IsConfigPathLazy
                        ? (IValueProvider) new ValueProviderProxy(() => ReadConfigGenericValue(argument))
                        : new ConstValueProvider(ReadConfigGenericValue(argument));

                    valueProvidersCollection.Add(
                        valueProvider
                    );
                }

                if (_commandConfig.CliConfig.Config != null)
                {
                    var valueProvider = argument.IsConfigPathLazy
                        ? (IValueProvider) new ValueProviderProxy(() => ReadConfigValue(argument))
                        : new ConstValueProvider(ReadConfigValue(argument));

                    valueProvidersCollection.Add(
                        valueProvider
                    );
                }
            }

            if (argument.EnvironmentVariable != null)
            {
                var valueProvider = argument.IsEnvironmentVariableLazy
                    ? (IValueProvider) new ValueProviderProxy(() => ReadEnvironmentVariable(argument))
                    : new ConstValueProvider(ReadEnvironmentVariable(argument));

                valueProvidersCollection.Add(valueProvider);
            }

            return valueProvidersCollection;
        }

        private object ReadConfigValue(DefaultValueArgument argument)
        {
            return ReadValue(argument, _commandConfig.CliConfig.Config?.Get(argument.ConfigPath));
        }

        private object ReadConfigGenericValue(DefaultValueArgument argument)
        {
            if (_commandConfig.CliConfig.GenericConfig == null)
                return null;

            if (!_commandConfig.CliConfig.GenericConfig.Has(argument.ConfigPath))
                return null;

            return _commandConfig.CliConfig.GenericConfig.GetType()
                .GetMethod("Get", BindingFlags.Public | BindingFlags.Instance)
                ?.MakeGenericMethod(argument.ValueType)
                .Invoke(_commandConfig.CliConfig.GenericConfig, new object[] { argument.ConfigPath });
        }

        private object ReadValue(Argument argument, string value)
        {
            if (value == null)
                return null;

            if (argument.ValueType.IsArray)
            {
                var values = value.Split(';')
                    .Select(v => ConvertValueToExpectedType(v, argument.ValueType.GetElementType()))
                    .ToArray();

                return ArrayUtils.GetArray(
                    values,
                    argument.ValueType);
            }

            return ConvertValueToExpectedType(value, argument.ValueType);
        }

        private object ReadEnvironmentVariable(DefaultValueArgument argument)
        {
            return ReadValue(argument, _commandConfig.CliConfig.Environment.Get(argument.EnvironmentVariable));
        }

        private void ReadParam(string arg)
        {
            if (_parameters.HasData())
            {
                var parameter = _parameters.Shift();
                _commandArgs.AddParameterValueProvider(parameter.Name, new ConstValueProvider(ConvertValueToExpectedType(arg, parameter.ValueType)));
            }
            else if (_parameterSeries != null)
                _commandArgs.AddParameterToSeries(ConvertValueToExpectedType(arg, _parameterSeries.ValueType));
            else
                throw new TooManyParametersException(arg);
        }

        private bool ReadOpt(string arg)
        {
            var opt = _commandConfig.Options.FirstOrDefault(
                option => ArgMatcher.GetMatcher(_commandConfig.CliConfig.Dialect).IsOptionMatching(option, arg)
            );

            if (opt == null)
                return false;

            if (!opt.RequireValue)
            {
                if (opt.ValueType == typeof(bool))
                    _commandArgs.AddOptionValueProvider(opt.Name, new ConstValueProvider(!(bool) opt.DefaultValue));

                return true;
            }

            var optionValue = OptionReader
                .GetReader(_commandConfig.CliConfig.Dialect.OptionValueMode)
                .ReadValue(_args, arg);

            if (opt.ValueType.IsArray)
            {
                if (_optionValues.ContainsKey(opt.Name))
                {
                    _optionValues[opt.Name].Add(ConvertValueToExpectedType(optionValue, opt.ValueType.GetElementType()));
                }
                else if (opt.ValueType.IsArray)
                {
                    _optionValues.Add(opt.Name,
                        new ArrayAggregator(ConvertValueToExpectedType(optionValue, opt.ValueType.GetElementType())));
                }
            }
            else
            {
                _commandArgs.AddOptionValueProvider(opt.Name,
                        new ConstValueProvider(ConvertValueToExpectedType(optionValue, opt.ValueType)));
            }

            return true;
        }

        private object ConvertValueToExpectedType(string value, Type expectedType)
        {
            if (value == null)
                return null;

            if (expectedType == typeof(string))
                return value;

            foreach (var converter in _commandConfig.CliConfig.ArgumentConverters)
                if (converter.CanConvert(expectedType))
                    return converter.Convert(expectedType, value);

            throw new MissingConverterException(expectedType);
        }

        private bool CurrentArgIsCommand()
        {
            return _args.HasData() && _commandConfig.Commands.Any(command => command.Name == _args.Current());
        }

        private CommandExecutionContext GetDefaultCommand()
        {
            return new CommandExecutionContextProvider(
                _commandConfig.DefaultCommand?.GetCommandConfig(_commandConfig),
                _args.ToArray()
            ).GetFromArgs();
        }

        private bool HasDefaultCommand()
        {
            return _commandConfig.DefaultCommand != null;
        }
    }
}