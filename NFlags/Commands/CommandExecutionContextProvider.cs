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
            _parameters = new Shifter<Parameter>(commandConfig.Parameters.ToArray());
            _parameterSeries = commandConfig.ParameterSeries;
            _args = new Shifter<string>(args);
            _commandArgs = InitDefaultCommandArgs();
            _optionValues = new Dictionary<string, ArrayAggregator>();
        }

        public CommandExecutionContext GetFromArgs()
        {
            var cmdAllowed = true;
            while (_args.HasData())
            {
                var arg = _args.Shift();

                Command cmd = null;
                if (cmdAllowed)
                    cmd = GetCommand(arg);

                if (cmd == null)
                {
                    cmd = GetDefaultCommand();

                    if (cmd != null)
                        _args.ShiftBack();
                }

                if (cmd != null)
                    return cmd.Read(_args.ToArray());

                if (_commandConfig.PrintHelpOnExecute)
                    return PrepareHelpCommandExecutionContext(_commandConfig);

                cmdAllowed = false;
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


            var noArgsDefaultCommand = GetDefaultCommand();
            if (noArgsDefaultCommand != null)
                return noArgsDefaultCommand.Read(_args.ToArray());

            return _commandConfig.PrintHelpOnExecute
                ? PrepareHelpCommandExecutionContext(_commandConfig)
                : new CommandExecutionContext(_commandConfig.Execute, _commandArgs);
        }


        public static PrintHelpCommandExecutionContext PrepareHelpCommandExecutionContext(CommandConfig commandConfig, string additionalPrefixMessage = "")
        {
            return new PrintHelpCommandExecutionContext(additionalPrefixMessage, commandConfig);
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
                if (_commandConfig.NFlagsConfig.GenericConfig != null)
                {
                    var valueProvider = argument.IsConfigPathLazy
                        ? (IValueProvider) new ValueProviderProxy(() => ReadConfigGenericValue(argument))
                        : new ConstValueProvider(ReadConfigGenericValue(argument));

                    valueProvidersCollection.Add(
                        valueProvider
                    );
                }

                if (_commandConfig.NFlagsConfig.Config != null)
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
            return ReadValue(argument, _commandConfig.NFlagsConfig.Config?.Get(argument.ConfigPath));
        }

        private object ReadConfigGenericValue(DefaultValueArgument argument)
        {
            if (_commandConfig.NFlagsConfig.GenericConfig == null) 
                return null;
            
            if (!_commandConfig.NFlagsConfig.GenericConfig.Has(argument.ConfigPath))
                return null;                

            return _commandConfig.NFlagsConfig.GenericConfig.GetType()
                .GetMethod("Get", BindingFlags.Public | BindingFlags.Instance)
                ?.MakeGenericMethod(argument.ValueType)
                .Invoke(_commandConfig.NFlagsConfig.GenericConfig, new object[] { argument.ConfigPath });
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
            return ReadValue(argument, _commandConfig.NFlagsConfig.Environment.Get(argument.EnvironmentVariable));
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
                option => ArgMatcher.GetMatcher(_commandConfig.NFlagsConfig.Dialect).IsOptionMatching(option, arg)
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
                .GetReader(_commandConfig.NFlagsConfig.Dialect.OptionValueMode)
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

            foreach (var converter in _commandConfig.NFlagsConfig.ArgumentConverters)
                if (converter.CanConvert(expectedType))
                    return converter.Convert(expectedType, value);

            throw new MissingConverterException(expectedType);
        }

        private Command GetCommand(string arg)
        {
            return _commandConfig.Commands.FirstOrDefault(
                command => command.Name == arg
            )?.CreateCommand(_commandConfig);
        }

        private Command GetDefaultCommand()
        {
            return _commandConfig.DefaultCommand?.CreateCommand(_commandConfig);
        }
    }
}