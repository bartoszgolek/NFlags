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

        public CommandExecutionContextProvider(
            CommandConfig commandConfig,
            string[] args
        )
        {
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

            var commandConfig = _commandConfig;
            var args = new Shifter<string>(_args);

            while (args.HasData() && ArgIsCommand(commandConfig, args.Current()) || HasDefaultCommand(commandConfig))
            {
                if (args.HasData() && ArgIsCommand(commandConfig, args.Current()))
                {
                    commandConfig = GetSubCommandConfig(commandConfig, args.Current());
                    args.Next();
                }

                if (HasDefaultCommand(commandConfig))
                    commandConfig = GetDefaultCommandConfig(commandConfig);
            }
            
            InitBaseOptions(commandConfig);
            var commandArgs = InitDefaultCommandArgs(commandConfig);

            ReadArgsAndOptions(commandConfig, commandArgs, args);

            if (commandConfig.CliConfig.VersionConfig.Enabled && commandArgs.GetFlag(commandConfig.CliConfig.VersionConfig.Flag))
                return PreparePrintVersionCommandExecutionContext(commandConfig);

            if (commandConfig.PrintHelpOnExecute || commandArgs.GetFlag(commandConfig.CliConfig.HelpConfig.Flag))
                return PrepareHelpCommandExecutionContext(commandConfig);

            return new CommandExecutionContext(commandConfig.Execute, commandArgs);
        }

        private static void InitBaseOptions(CommandConfig commandConfig)
        {
            commandConfig.Options.Add(
                new Flag
                {
                    Name = commandConfig.CliConfig.HelpConfig.Flag,
                    Abr = commandConfig.CliConfig.HelpConfig.Abr,
                    Description = commandConfig.CliConfig.HelpConfig.Description,
                    DefaultValue = false
                }
            );

            if (commandConfig.CliConfig.VersionConfig.Enabled)
            {
                commandConfig.Options.Add(
                    new Flag
                    {
                        Name = commandConfig.CliConfig.VersionConfig.Flag,
                        Abr = commandConfig.CliConfig.VersionConfig.Abr,
                        Description = commandConfig.CliConfig.VersionConfig.Description,
                        DefaultValue = false
                    }
                );
            }
        }

        private CommandConfig GetSubCommandConfig(CommandConfig commandConfig, string commandName)
        {
            return commandConfig
                .Commands
                .FirstOrDefault(command => command.Name == commandName)
                ?.GetCommandConfig(commandConfig);
        }

        private void ReadArgsAndOptions(CommandConfig commandConfig, CommandArgs commandArgs, Shifter<string> args)
        {
            var parameters = new Shifter<Parameter>(commandConfig.Parameters.ToArray());
            var optionValues = new Dictionary<string, ArrayAggregator>();
            while (args.HasData())
            {
                var arg = args.Shift();

                if (!ReadOpt(commandConfig, commandArgs, optionValues, args, arg) &&
                    !ReadParam(commandConfig, commandArgs, parameters, arg) &&
                    !ReadParamSeries(commandConfig, commandArgs, arg))
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
                            commandConfig.Options.Single(o => o.Name == optionValue.Key).ValueType)
                    )
                );
            }
        }

        private static bool ReadParamSeries(CommandConfig commandConfig, CommandArgs commandArgs, string arg)
        {
            if (commandConfig.ParameterSeries == null)
                return false;
            
            commandArgs.AddParameterToSeries(ConvertValueToExpectedType(commandConfig, arg, commandConfig.ParameterSeries.ValueType));
            return true;
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

        private CommandArgs InitDefaultCommandArgs(CommandConfig commandConfig)
        {
            var commandArgs = new CommandArgs();
            foreach (var option in commandConfig.Options)
            {
                var valueProviders = GetDefaultValueProvidersInPrecedence(commandConfig, option);
                foreach (var valueProvider in valueProviders)
                    commandArgs.AddOptionValueProvider(option.Name, valueProvider);
            }

            foreach (var parameter in commandConfig.Parameters)
            {
                var valueProviders = GetDefaultValueProvidersInPrecedence(commandConfig, parameter);
                foreach (var valueProvider in valueProviders)
                    commandArgs.AddParameterValueProvider(parameter.Name, valueProvider);
            }

            return commandArgs;
        }

        private IEnumerable<IValueProvider> GetDefaultValueProvidersInPrecedence(CommandConfig commandConfig,
            DefaultValueArgument argument)
        {
            var valueProvidersCollection = new List<IValueProvider>
            {
                new ConstValueProvider(argument.DefaultValue)
            };

            if (argument.ConfigPath != null)
            {
                if (commandConfig.CliConfig.GenericConfig != null)
                {
                    var valueProvider = argument.IsConfigPathLazy
                        ? (IValueProvider) new ValueProviderProxy(() => ReadConfigGenericValue(commandConfig, argument))
                        : new ConstValueProvider(ReadConfigGenericValue(commandConfig, argument));

                    valueProvidersCollection.Add(
                        valueProvider
                    );
                }

                if (commandConfig.CliConfig.Config != null)
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

        private object ReadConfigGenericValue(CommandConfig commandConfig, DefaultValueArgument argument)
        {
            if (commandConfig.CliConfig.GenericConfig == null)
                return null;

            if (!commandConfig.CliConfig.GenericConfig.Has(argument.ConfigPath))
                return null;

            return commandConfig.CliConfig.GenericConfig.GetType()
                .GetMethod("Get", BindingFlags.Public | BindingFlags.Instance)
                ?.MakeGenericMethod(argument.ValueType)
                .Invoke(commandConfig.CliConfig.GenericConfig, new object[] { argument.ConfigPath });
        }

        private object ReadValue(Argument argument, string value)
        {
            if (value == null)
                return null;

            if (argument.ValueType.IsArray)
            {
                var values = value.Split(';')
                    .Select(v => ConvertValueToExpectedType(_commandConfig, v, argument.ValueType.GetElementType()))
                    .ToArray();

                return ArrayUtils.GetArray(
                    values,
                    argument.ValueType);
            }

            return ConvertValueToExpectedType(_commandConfig, value, argument.ValueType);
        }

        private object ReadEnvironmentVariable(DefaultValueArgument argument)
        {
            return ReadValue(argument, _commandConfig.CliConfig.Environment.Get(argument.EnvironmentVariable));
        }

        private bool ReadParam(CommandConfig commandConfig, CommandArgs commandArgs, Shifter<Parameter> parameters, string arg)
        {
            if (!parameters.HasData())
                return false;
            
            var parameter = parameters.Shift();
            commandArgs.AddParameterValueProvider(parameter.Name, new ConstValueProvider(ConvertValueToExpectedType(commandConfig, arg, parameter.ValueType)));
            return true;
        }

        private bool ReadOpt(CommandConfig commandConfig, CommandArgs commandArgs, Dictionary<string, ArrayAggregator> optionValues, Shifter<string> args, string arg)
        {
            var opt = commandConfig.Options.FirstOrDefault(
                option => ArgMatcher.GetMatcher(commandConfig.CliConfig.Dialect).IsOptionMatching(option, arg)
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
                .GetReader(_commandConfig.CliConfig.Dialect.OptionValueMode)
                .ReadValue(args, arg);

            if (opt.ValueType.IsArray)
            {
                if (optionValues.ContainsKey(opt.Name))
                {
                    optionValues[opt.Name].Add(ConvertValueToExpectedType(commandConfig, optionValue, opt.ValueType.GetElementType()));
                }
                else if (opt.ValueType.IsArray)
                {
                    optionValues.Add(opt.Name,
                        new ArrayAggregator(ConvertValueToExpectedType(commandConfig, optionValue, opt.ValueType.GetElementType())));
                }
            }
            else
            {
                commandArgs.AddOptionValueProvider(opt.Name,
                        new ConstValueProvider(ConvertValueToExpectedType(commandConfig, optionValue, opt.ValueType)));
            }

            return true;
        }

        private static object ConvertValueToExpectedType(CommandConfig commandConfig, string value, Type expectedType)
        {
            if (value == null)
                return null;

            if (expectedType == typeof(string))
                return value;

            foreach (var converter in commandConfig.CliConfig.ArgumentConverters)
                if (converter.CanConvert(expectedType))
                    return converter.Convert(expectedType, value);

            throw new MissingConverterException(expectedType);
        }

        private static bool ArgIsCommand(CommandConfig commandConfig, string arg)
        {
            return commandConfig.Commands.Any(command => command.Name == arg);
        }

        private CommandConfig GetDefaultCommandConfig(CommandConfig commandConfig)
        {
            return commandConfig.DefaultCommand?.GetCommandConfig(commandConfig);
        }

        private bool HasDefaultCommand(CommandConfig commandConfig)
        {
            return commandConfig.DefaultCommand != null;
        }
    }
}