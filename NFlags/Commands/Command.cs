using System.Linq;
using NFlags.Arguments;
using NFlags.Utils;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents registered application command.
    /// </summary>
    public class Command
    {
        private const string HelpFlag = "help";
        private const string HelpFlagAbr = "h";
        private const string HelpDescription = "Prints this help";

        private readonly CommandConfig _commandConfig;

        /// <summary>
        /// Creates new command instance.
        /// </summary>
        /// <param name="commandConfig">Command configuration.</param>
        public Command(CommandConfig commandConfig)
        {
            _commandConfig = commandConfig;
        }

        /// <summary>
        /// Read and parse arguments.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        public CommandExecutionContext Read(string[] args)
        {
            _commandConfig.Flags.Add(
                new Flag
                {
                    Name = HelpFlag,
                    Abr = HelpFlagAbr,
                    Description = HelpDescription,
                    DefaultValue = false
                }
            );

            var commandExecutionContext = new CommandInt(
                _commandConfig,
                args
            ).Read();

            return commandExecutionContext.Args != null && commandExecutionContext.Args.Flags[HelpFlag]
                ? new CommandExecutionContext((commandArgs, output) => output(PrintHelp()), null)
                : commandExecutionContext;
        }

        private string PrintHelp()
        {
            return new HelpPrinter(_commandConfig).Print();
        }

        private class CommandInt
        {
            private readonly CommandConfig _commandConfig;
            private readonly Shifter<Parameter> _parameters;
            private readonly Shifter<string> _args;
            private readonly CommandArgs _commandArgs;

            public CommandInt(
                CommandConfig commandConfig,
                string[] args)
            {
                _commandConfig = commandConfig;
                _parameters = new Shifter<Parameter>(commandConfig.Parameters.ToArray());
                _args = new Shifter<string>(args);
                _commandArgs = InitDefaultCommandArgs();
            }

            public CommandExecutionContext Read()
            {
                var cmdAllowed = true;
                while (_args.HasData())
                {
                    var arg = _args.Shift();

                    Command cmd = null;
                    if (cmdAllowed)
                        cmd = GetCommand(arg);

                    if (cmd != null)
                        return cmd.Read(_args.ToArray());

                    cmdAllowed = false;
                    if (!ReadOpt(arg) && !ReadFlag(arg))
                        ReadParam(arg);

                }

                return new CommandExecutionContext(_commandConfig.Execute, _commandArgs);
            }

            private CommandArgs InitDefaultCommandArgs()
            {
                var commandArgs = new CommandArgs();
                foreach (var flag in _commandConfig.Flags)
                    commandArgs.Flags.Add(flag.Name, flag.DefaultValue);

                foreach (var option in _commandConfig.Options)
                    commandArgs.Options.Add(option.Name, option.DefaultValue);

                foreach (var parameter in _parameters.ToArray())
                    commandArgs.Parameters.Add(parameter.Name, parameter.DefaultValue);

                return commandArgs;
            }

            private void ReadParam(string arg)
            {
                if (_parameters.HasData())
                    _commandArgs.Parameters[_parameters.Shift().Name] = arg;
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

                _commandArgs.Options[opt.Name] = OptionReader
                    .GetReader(_commandConfig.NFlagsConfig.Dialect.OptionValueMode)
                    .ReadValue(_args, arg);

                return true;
            }

            private Command GetCommand(string arg)
            {
                return _commandConfig.Commands.FirstOrDefault(
                    command => command.Name == arg
                )?.CreateCommand();
            }

            private bool ReadFlag(string arg)
            {
                var flag = _commandConfig.Flags.FirstOrDefault(
                    f => ArgMatcher.GetMatcher(_commandConfig.NFlagsConfig.Dialect).IsFlagMatching(f, arg)
                );

                if (flag == null)
                    return false;

                _commandArgs.Flags[flag.Name] = !flag.DefaultValue;

                return true;
            }
        }
    }
}