using System.Collections.Generic;
using System.Linq;
using NFlags.Arguments;
using NFlags.TypeConverters;
using NFlags.Utils;
using NFlags.ValueProviders;

namespace NFlags.Commands
{
    internal class CommandExecutionContextProvider
    {
        private readonly CommandConfig _rootCommandConfig;
        private readonly string[] _args;
        private readonly CliConfig _cliConfig;
        private readonly ValueConverter _valueConverter;
        private readonly ArgumentValueReader _argumentValueReader;
        private readonly ConfigReader _configReader;

        public CommandExecutionContextProvider(
            CliConfig cliConfig,
            CommandConfig rootCommandConfig,
            string[] args)
        {
            _cliConfig = cliConfig;

            _rootCommandConfig = rootCommandConfig;
            _args = args;

            _valueConverter = new ValueConverter(cliConfig.ArgumentConverters);
            _argumentValueReader = new ArgumentValueReader(_valueConverter);
            _configReader = new ConfigReader(_argumentValueReader, cliConfig.Config, cliConfig.GenericConfig);
        }

        public CommandExecutionContext GetFromArgs()
        {
            try
            {
                var argumentsReader = new ArrayReader<string>(_args);

                var commandConfig = ParseCommands(_rootCommandConfig, argumentsReader);
                var commandArguments = ReadCommandArguments(commandConfig, argumentsReader);

                return GetCommandExecutionContext(commandConfig, commandArguments);
            }
            catch (ArgumentValueException e)
            {
                if (!_cliConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(_rootCommandConfig, e.Message);
            }
            catch (TooManyParametersException e)
            {
                if (!_cliConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(_rootCommandConfig, e.Message);
            }
        }

        private CommandExecutionContext GetCommandExecutionContext(CommandConfig commandConfig, CommandArgs commandArgs)
        {
            if (_cliConfig.VersionConfig.Enabled && commandArgs.GetFlag(_cliConfig.VersionConfig.Flag))
                return PreparePrintVersionCommandExecutionContext(commandConfig);

            if (commandConfig.PrintHelpOnExecute || commandArgs.GetFlag(_cliConfig.HelpConfig.Flag))
                return PrepareHelpCommandExecutionContext(commandConfig);

            return new CommandExecutionContext(commandConfig.Execute, commandArgs);
        }

        private CommandConfig ParseCommands(CommandConfig commandConfig, ArrayReader<string> argumentsReader)
        {
            while (argumentsReader.HasData() && ArgIsCommand(commandConfig, argumentsReader.Current()) || HasDefaultCommand(commandConfig))
            {
                if (argumentsReader.HasData() && ArgIsCommand(commandConfig, argumentsReader.Current()))
                    commandConfig = GetSubCommandConfig(commandConfig, argumentsReader.Read());

                if (HasDefaultCommand(commandConfig))
                    commandConfig = GetDefaultCommandConfig(commandConfig);
            }

            InitBaseOptions(ref commandConfig);

            return commandConfig;
        }

        private void InitBaseOptions(ref CommandConfig commandConfig)
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

        private CommandArgs ReadCommandArguments(CommandConfig commandConfig, ArrayReader<string> argumentsReader)
        {
            var options = commandConfig.Options;
            var optionValues = new Dictionary<string, ArrayAggregator>();
            var parameterSeries = commandConfig.ParameterSeries;
            var parameters = new ArrayReader<Parameter>(commandConfig.Parameters.ToArray());

            var commandArgs = InitDefaultCommandArgs(commandConfig);
            while (argumentsReader.HasData())
            {
                var arg = argumentsReader.Read();

                if (!ReadOpt(options, commandArgs, optionValues, argumentsReader, arg) &&
                    !ReadParam(commandArgs, parameters, arg) &&
                    !ReadParamSeries(parameterSeries, commandArgs, arg))
                {
                    throw new TooManyParametersException(arg);
                }
            }

            foreach (var (optionName, optionValue) in optionValues)
            {
                commandArgs.AddOptionValueProvider(
                    optionName,
                    new ConstValueProvider(
                        optionValue.GetArray(
                            options.Single(o => o.Name == optionName).ValueType)
                    )
                );
            }

            return commandArgs;
        }

        private bool ReadParamSeries(ParameterSeries parameterSeries, CommandArgs commandArgs, string arg)
        {
            if (parameterSeries == null)
                return false;

            commandArgs.AddParameterToSeries(_valueConverter.ConvertValueToExpectedType(arg, parameterSeries.ValueType, parameterSeries.Converter));
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
                foreach (var valueProvider in GetDefaultValueProvidersInPrecedence(option))
                    commandArgs.AddOptionValueProvider(option.Name, valueProvider);
            }

            foreach (var parameter in commandConfig.Parameters)
            {
                foreach (var valueProvider in GetDefaultValueProvidersInPrecedence(parameter))
                    commandArgs.AddParameterValueProvider(parameter.Name, valueProvider);
            }

            return commandArgs;
        }

        private IEnumerable<IValueProvider> GetDefaultValueProvidersInPrecedence(DefaultValueArgument argument)
        {
            return new List<IValueProvider>
            {
                new ConstValueProvider(argument.DefaultValue),
                GetReadConfigValueProvider(argument),
                GetEnvironmentValueProvider(argument)
            };
        }

        private IValueProvider GetEnvironmentValueProvider(DefaultValueArgument argument)
        {
            if (argument.EnvironmentVariable == null)
                return new EmptyValueProvider();

            return argument.IsEnvironmentVariableLazy
                ? (IValueProvider) new ValueProviderProxy(() => ReadEnvironmentVariable(argument))
                : new ConstValueProvider(ReadEnvironmentVariable(argument));
        }

        private IValueProvider GetReadConfigValueProvider(DefaultValueArgument argument)
        {
            if (argument.ConfigPath == null)
                return new EmptyValueProvider();

            if (_cliConfig.GenericConfig != null)
            {
                return argument.IsConfigPathLazy
                    ? (IValueProvider) new ValueProviderProxy(() => _configReader.ReadConfigGenericValue(argument))
                    : new ConstValueProvider(_configReader.ReadConfigGenericValue(argument));
            }

            if (_cliConfig.Config != null)
            {
                return argument.IsConfigPathLazy
                    ? (IValueProvider) new ValueProviderProxy(() => _configReader.ReadConfigValue(argument))
                    : new ConstValueProvider(_configReader.ReadConfigValue(argument));
            }

            return new EmptyValueProvider();
        }

        private object ReadEnvironmentVariable(DefaultValueArgument argument)
        {
            return _argumentValueReader.Read(argument, _cliConfig.Environment.Get(argument.EnvironmentVariable));
        }

        private bool ReadParam(CommandArgs commandArgs, ArrayReader<Parameter> parameters, string arg)
        {
            if (!parameters.HasData())
                return false;

            var parameter = parameters.Read();
            commandArgs.AddParameterValueProvider(parameter.Name, new ConstValueProvider(_valueConverter.ConvertValueToExpectedType(arg, parameter.ValueType, parameter.Converter)));
            return true;
        }

        private bool ReadOpt(IEnumerable<PrefixedDefaultValueArgument> options, CommandArgs commandArgs, Dictionary<string, ArrayAggregator> optionValues, ArrayReader<string> args, string arg)
        {
            var opt = options.FirstOrDefault(
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
                    optionValues[opt.Name].Add(_valueConverter.ConvertValueToExpectedType(optionValue, opt.ValueType.GetElementType(), opt.Converter));
                }
                else if (opt.ValueType.IsArray)
                {
                    optionValues.Add(opt.Name,
                        new ArrayAggregator(_valueConverter.ConvertValueToExpectedType(optionValue, opt.ValueType.GetElementType(), opt.Converter)));
                }
            }
            else
            {
                commandArgs.AddOptionValueProvider(opt.Name,
                        new ConstValueProvider(_valueConverter.ConvertValueToExpectedType(optionValue, opt.ValueType, opt.Converter)));
            }

            return true;
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