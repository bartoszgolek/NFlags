using System;
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
        private readonly CommandConfig _commandConfig;
        private readonly Shifter<Parameter> _parameters;
        private readonly ParameterSeries _parameterSeries;
        private readonly Shifter<string> _args;
        private readonly CommandArgs _commandArgs;

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
                if (!ReadOpt(arg) && !ReadFlag(arg))
                    ReadParam(arg);

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
            foreach (var flag in _commandConfig.Flags)
            {
                var valueProviders = GetDefaultValueProvidersInPrecedence(flag);
                foreach (var valueProvider in valueProviders)
                    commandArgs.AddFlagValueProvider(flag.Name, valueProvider);
            }

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
                var config = _commandConfig.NFlagsConfig.Config;
                var value = config?.Get(argument.ConfigPath);
                if (value != null)
                {
                    valueProvidersCollection.Add(
                        new ConstValueProvider(ConvertValueToExpectedType(value, argument.ValueType))
                    );
                }
            }

            if (argument.EnvironmentVariable != null)
            {

                var valueProvider = argument.IsEnvironmentVariableLazy
                    ? (IValueProvider) new ValueProviderProxy(() => ConvertValueToExpectedType(_commandConfig.NFlagsConfig.Environment.Get(argument.EnvironmentVariable), argument.ValueType))
                    : new ConstValueProvider(ConvertValueToExpectedType(_commandConfig.NFlagsConfig.Environment.Get(argument.EnvironmentVariable), argument.ValueType));

                valueProvidersCollection.Add(valueProvider);
            }

            return valueProvidersCollection;
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

            var optionValue = OptionReader
                .GetReader(_commandConfig.NFlagsConfig.Dialect.OptionValueMode)
                .ReadValue(_args, arg);

            _commandArgs.AddOptionValueProvider(opt.Name, new ConstValueProvider(ConvertValueToExpectedType(optionValue, opt.ValueType)));

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

        private bool ReadFlag(string arg)
        {
            var flag = _commandConfig.Flags.FirstOrDefault(
                f => ArgMatcher.GetMatcher(_commandConfig.NFlagsConfig.Dialect).IsFlagMatching(f, arg)
            );

            if (flag == null)
                return false;

            _commandArgs.AddFlagValueProvider(flag.Name, new ConstValueProvider(!(bool)flag.DefaultValue));

            return true;
        }
    }
}