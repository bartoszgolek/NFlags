using System.Collections.Generic;
using System.Linq;
using NFlags.Utils;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents registered application command.
    /// </summary>
    public class Command
    {
        private readonly NFlagsConfig _nFlagsConfig;

        private readonly CommandConfig _commandConfig;

        /// <summary>
        /// Creates new command instance.
        /// </summary>
        /// <param name="nFlagsConfig">Configuration od param reader.</param>
        /// <param name="commandConfig">Command configuration.</param>
        public Command(NFlagsConfig nFlagsConfig, CommandConfig commandConfig)
        {
            _nFlagsConfig = nFlagsConfig;
            _commandConfig = commandConfig;
        }
        
        /// <summary>
        /// Read and parse arguments.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        public void Read(string[] args)
        {
            new CommandInt(
                _nFlagsConfig.Dialect,
                _commandConfig.Commands,
                _commandConfig.Flags,
                _commandConfig.Parameters,
                _commandConfig.Options,
                InitDefaultCommandArgs(),
                args
            ).Read();
        }

        /// <summary>
        /// Print and returns help for application.
        /// </summary>
        /// <returns>Help text</returns>
        public string PrintHelp()
        {
            return new HelpPrinter(_nFlagsConfig, _commandConfig).Print();
        }

        private CommandArgs InitDefaultCommandArgs()
        {
            var commandArgs = new CommandArgs();
            foreach (var flag in _commandConfig.Flags)
                commandArgs.Flags.Add(flag.Name, flag.DefaultValue);
                
            foreach (var option in _commandConfig.Options)
                commandArgs.Options.Add(option.Name, option.DefaultValue);

            foreach (var parameter in _commandConfig.Parameters.ToArray())
                commandArgs.Parameters.Add(parameter.Name, parameter.DefaultValue);

            return commandArgs;
        }

        private class CommandInt
        {
            private readonly Dialect _dialect;
            private readonly List<Flag> _flags;
            private readonly Shifter<Parameter> _parameters;
            private readonly List<Option> _options;
            private readonly List<Command> _commands;
            private readonly Shifter<string> _args;
            private readonly CommandArgs _commandArgs;

            public CommandInt(
                Dialect dialect, 
                List<Command> commands, 
                List<Flag> flags, 
                List<Parameter> parameters, 
                List<Option> options, 
                CommandArgs commandArgs,
                string[] args)
            {
                _dialect = dialect;
                _commands = commands;
                _flags = flags;
                _parameters = new Shifter<Parameter>(parameters.ToArray());
                _options = options;
                _args = new Shifter<string>(args);
                _commandArgs = commandArgs;
            }

            public void Read()
            {
                var cmdAllowed = true;
                while (_args.HasData())
                {
                    var arg = _args.Shift();

                    Command cmd = null;
                    if (cmdAllowed)
                        cmd = GetCommand(arg);
                    
                    if (cmd != null)
                        cmd.Read(_args.ToArray());
                    else
                    {
                        cmdAllowed = false;
                        if (!ReadOpt(arg) && !ReadFlag(arg))
                            ReadParam(arg);
                    }
                }
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
                var opt = _options.FirstOrDefault(
                    option => ArgMatcher.GetMatcher(_dialect).IsOptionMatching(option, arg)
                );

                if (opt == null)
                    return false;

                _commandArgs.Options[opt.Name] = OptionReader.GetReader(_dialect.OptionValueMode).ReadValue(_args, arg);

                return true;
            }

            private Command GetCommand(string arg)
            {
                return _commands.FirstOrDefault(
                    command => command._commandConfig.Name == arg
                );
            }

            private bool ReadFlag(string arg)
            {
                var flag = _flags.FirstOrDefault(
                    f => ArgMatcher.GetMatcher(_dialect).IsFlagMatching(f, arg)
                );

                if (flag == null)
                    return false;

                _commandArgs.Flags[flag.Name] = !flag.DefaultValue;

                return true;
            }
        }
    }
}