using System;
using System.Linq;
using NFlags.Arguments;
using NFlags.TypeConverters;
using NFlags.Utils;

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

                cmdAllowed = false;
                if (!ReadOpt(arg) && !ReadFlag(arg))
                    ReadParam(arg);

            }
            
            var noArgsDefaultCommand = GetDefaultCommand();
            return noArgsDefaultCommand != null 
                ? noArgsDefaultCommand.Read(_args.ToArray())
                : new CommandExecutionContext(_commandConfig.Execute, _commandArgs);
        }

        private CommandArgs InitDefaultCommandArgs()
        {
            var commandArgs = new CommandArgs();
            foreach (var flag in _commandConfig.Flags)
                commandArgs.AddFlag(flag.Name, flag.GetDefault());

            foreach (var option in _commandConfig.Options)
                commandArgs.AddOption(option.Name, option.DefaultValue);

            foreach (var parameter in _parameters.ToArray())
                commandArgs.AddParameter(parameter.Name, parameter.DefaultValue);

            return commandArgs;
        }

        private void ReadParam(string arg)
        {
            if (_parameters.HasData())
            {
                var parameter = _parameters.Shift();
                _commandArgs.AddParameter(parameter.Name, ConvertValueToExpectedType(arg, parameter.ValueType));
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

            _commandArgs.AddOption(opt.Name, ConvertValueToExpectedType(optionValue, opt.ValueType));

            return true;
        }

        private object ConvertValueToExpectedType(string value, Type expectedType)
        {
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

            _commandArgs.AddFlag(flag.Name, !flag.GetDefault());

            return true;
        }
    }
}