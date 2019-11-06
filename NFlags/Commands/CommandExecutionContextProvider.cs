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
        private readonly string[] _args;
        private readonly CliConfig _cliConfig;

        public CommandExecutionContextProvider(
            CliConfig cliConfig,
            CommandConfig commandConfig,
            string[] args)
        {
            _cliConfig = cliConfig;
            _commandConfig = commandConfig;
            _args = args;
        }

        public CommandExecutionContext GetFromArgs()
        {
            try
            {
                return PrepareCommandExecutionContext();
            }
            catch (ArgumentValueException e)
            {
                if (!_cliConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(_commandConfig, e.Message);
            }
            catch (TooManyParametersException e)
            {
                if (!_cliConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(_commandConfig, e.Message);
            }
        }

        private CommandExecutionContext PrepareCommandExecutionContext()
        {
            var commandArgsParseContext = ParseCommands(_commandConfig, _args);

            InitBaseOptions(commandArgsParseContext.CommandConfig);
            var commandArgs = InitDefaultCommandArgs(commandArgsParseContext.CommandConfig);

            ReadArgsAndOptions(commandArgsParseContext, commandArgs);

            if (_cliConfig.VersionConfig.Enabled && commandArgs.GetFlag(_cliConfig.VersionConfig.Flag))
                return PreparePrintVersionCommandExecutionContext(commandArgsParseContext.CommandConfig);

            if (commandArgsParseContext.CommandConfig.PrintHelpOnExecute || commandArgs.GetFlag(_cliConfig.HelpConfig.Flag))
                return PrepareHelpCommandExecutionContext(commandArgsParseContext.CommandConfig);

            return new CommandExecutionContext(commandArgsParseContext.CommandConfig.Execute, commandArgs);
        }

        private static CommandArgsParseContext ParseCommands(CommandConfig commandConfig, string[] args)
        {
            var argReader = new ArrayReader<string>(args);
            while (argReader.HasData() && ArgIsCommand(commandConfig, argReader.Current()) || HasDefaultCommand(commandConfig))
            {
                if (argReader.HasData() && ArgIsCommand(commandConfig, argReader.Current()))
                    commandConfig = GetSubCommandConfig(commandConfig, argReader.Read());

                if (HasDefaultCommand(commandConfig))
                    commandConfig = GetDefaultCommandConfig(commandConfig);
            }

            return new CommandArgsParseContext(commandConfig, argReader.ReadToEnd());
        }

        private void InitBaseOptions(CommandConfig commandConfig)
        {
            commandConfig.Options.Add(
                new Flag
                {
                    Name = _cliConfig.HelpConfig.Flag,
                    Abr = _cliConfig.HelpConfig.Abr,
                    Description = _cliConfig.HelpConfig.Description,
                    DefaultValue = false
                }
            );

            if (_cliConfig.VersionConfig.Enabled)
            {
                commandConfig.Options.Add(
                    new Flag
                    {
                        Name = _cliConfig.VersionConfig.Flag,
                        Abr = _cliConfig.VersionConfig.Abr,
                        Description = _cliConfig.VersionConfig.Description,
                        DefaultValue = false
                    }
                );
            }
        }

        private static CommandConfig GetSubCommandConfig(CommandConfig commandConfig, string commandName)
        {
            return commandConfig
                .Commands
                .FirstOrDefault(command => command.Name == commandName)
                ?.GetCommandConfig(commandConfig);
        }

        private void ReadArgsAndOptions(CommandArgsParseContext commandArgsParseContext, CommandArgs commandArgs)
        {
            var args = new ArrayReader<string>(commandArgsParseContext.Args);
            var parameters = new ArrayReader<Parameter>(commandArgsParseContext.CommandConfig.Parameters.ToArray());
            var optionValues = new Dictionary<string, ArrayAggregator>();
            while (args.HasData())
            {
                var arg = args.Read();

                if (!ReadOpt(commandArgsParseContext.CommandConfig, commandArgs, optionValues, args, arg) &&
                    !ReadParam(commandArgs, parameters, arg) &&
                    !ReadParamSeries(commandArgsParseContext.CommandConfig, commandArgs, arg))
                {
                    throw new TooManyParametersException(arg);
                }
            }

            foreach (var optionValue in optionValues)
            {
                commandArgs.AddOptionValueProvider(
                    optionValue.Key,
                    new ConstValueProvider(
                        optionValue.Value.GetArray(
                            commandArgsParseContext.CommandConfig.Options.Single(o => o.Name == optionValue.Key).ValueType)
                    )
                );
            }
        }

        private bool ReadParamSeries(CommandConfig commandConfig, CommandArgs commandArgs, string arg)
        {
            if (commandConfig.ParameterSeries == null)
                return false;
            
            commandArgs.AddParameterToSeries(ConvertValueToExpectedType(_cliConfig.ArgumentConverters, arg, commandConfig.ParameterSeries.ValueType));
            return true;
        }

        private PrintHelpCommandExecutionContext PrepareHelpCommandExecutionContext(CommandConfig commandConfig, string additionalPrefixMessage)
        {
            return new PrintHelpCommandExecutionContext(additionalPrefixMessage, _cliConfig, commandConfig);
        }


        private PrintHelpCommandExecutionContext PrepareHelpCommandExecutionContext(CommandConfig commandConfig)
        {
            return new PrintHelpCommandExecutionContext(_cliConfig, commandConfig);
        }


        private PrintVersionCommandExecutionContext PreparePrintVersionCommandExecutionContext(CommandConfig commandConfig)
        {
            return new PrintVersionCommandExecutionContext(_cliConfig, commandConfig);
        }

        private CommandArgs InitDefaultCommandArgs(CommandConfig commandConfig)
        {
            var commandArgs = new CommandArgs();
            foreach (var option in commandConfig.Options)
            {
                var valueProviders = GetDefaultValueProvidersInPrecedence(option);
                foreach (var valueProvider in valueProviders)
                    commandArgs.AddOptionValueProvider(option.Name, valueProvider);
            }

            foreach (var parameter in commandConfig.Parameters)
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
                if (_cliConfig.GenericConfig != null)
                {
                    var valueProvider = argument.IsConfigPathLazy
                        ? (IValueProvider) new ValueProviderProxy(() => ReadConfigGenericValue(argument))
                        : new ConstValueProvider(ReadConfigGenericValue(argument));

                    valueProvidersCollection.Add(
                        valueProvider
                    );
                }

                if (_cliConfig.Config != null)
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
            return ReadValue(argument, _cliConfig.Config?.Get(argument.ConfigPath));
        }

        private object ReadConfigGenericValue(DefaultValueArgument argument)
        {
            if (_cliConfig.GenericConfig == null)
                return null;

            if (!_cliConfig.GenericConfig.Has(argument.ConfigPath))
                return null;

            return _cliConfig.GenericConfig.GetType()
                .GetMethod("Get", BindingFlags.Public | BindingFlags.Instance)
                ?.MakeGenericMethod(argument.ValueType)
                .Invoke(_cliConfig.GenericConfig, new object[] { argument.ConfigPath });
        }

        private object ReadValue(Argument argument, string value)
        {
            if (value == null)
                return null;

            if (argument.ValueType.IsArray)
            {
                var values = value.Split(';')
                    .Select(v => ConvertValueToExpectedType(_cliConfig.ArgumentConverters, v, argument.ValueType.GetElementType()))
                    .ToArray();

                return ArrayUtils.GetArray(
                    values,
                    argument.ValueType);
            }

            return ConvertValueToExpectedType(_cliConfig.ArgumentConverters, value, argument.ValueType);
        }

        private object ReadEnvironmentVariable(DefaultValueArgument argument)
        {
            return ReadValue(argument, _cliConfig.Environment.Get(argument.EnvironmentVariable));
        }

        private bool ReadParam(CommandArgs commandArgs, ArrayReader<Parameter> parameters, string arg)
        {
            if (!parameters.HasData())
                return false;
            
            var parameter = parameters.Read();
            commandArgs.AddParameterValueProvider(parameter.Name, new ConstValueProvider(ConvertValueToExpectedType(_cliConfig.ArgumentConverters, arg, parameter.ValueType)));
            return true;
        }

        private bool ReadOpt(CommandConfig commandConfig, CommandArgs commandArgs, Dictionary<string, ArrayAggregator> optionValues, ArrayReader<string> args, string arg)
        {
            var opt = commandConfig.Options.FirstOrDefault(
                option => ArgMatcher.GetMatcher(_cliConfig.Dialect).IsOptionMatching(option, arg)
            );

            if (opt == null)
                return false;

            if (!opt.RequireValue)
            {
                if (opt.ValueType == typeof(bool))
                    commandArgs.AddOptionValueProvider(opt.Name, new ConstValueProvider(!(bool) opt.DefaultValue));

                return true;
            }

            var optionValue = OptionReader
                .GetReader(_cliConfig.Dialect.OptionValueMode)
                .ReadValue(args, arg);

            if (opt.ValueType.IsArray)
            {
                if (optionValues.ContainsKey(opt.Name))
                {
                    optionValues[opt.Name].Add(ConvertValueToExpectedType(_cliConfig.ArgumentConverters, optionValue, opt.ValueType.GetElementType()));
                }
                else if (opt.ValueType.IsArray)
                {
                    optionValues.Add(opt.Name,
                        new ArrayAggregator(ConvertValueToExpectedType(_cliConfig.ArgumentConverters, optionValue, opt.ValueType.GetElementType())));
                }
            }
            else
            {
                commandArgs.AddOptionValueProvider(opt.Name,
                        new ConstValueProvider(ConvertValueToExpectedType(_cliConfig.ArgumentConverters, optionValue, opt.ValueType)));
            }

            return true;
        }

        private static object ConvertValueToExpectedType(IEnumerable<IArgumentConverter> argumentConverters, string value, Type expectedType)
        {
            if (value == null)
                return null;

            if (expectedType == typeof(string))
                return value;

            foreach (var converter in argumentConverters)
                if (converter.CanConvert(expectedType))
                    return converter.Convert(expectedType, value);

            throw new MissingConverterException(expectedType);
        }

        private static bool ArgIsCommand(CommandConfig commandConfig, string arg)
        {
            return commandConfig.Commands.Any(command => command.Name == arg);
        }

        private static CommandConfig GetDefaultCommandConfig(CommandConfig commandConfig)
        {
            return commandConfig.DefaultCommand?.GetCommandConfig(commandConfig);
        }

        private static bool HasDefaultCommand(CommandConfig commandConfig)
        {
            return commandConfig.DefaultCommand != null;
        }
    }
}