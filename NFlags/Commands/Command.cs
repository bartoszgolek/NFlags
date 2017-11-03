using System;
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
        private const string HelpFlag = "help";
        private const string HelpFlagAbr = "h";
        private const string HelpDescription = "Prints this help";
        
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
        /// Command name
        /// </summary>
        public string Name => _commandConfig.Name;

        /// <summary>
        /// Command description
        /// </summary>
        public string Description => _commandConfig.Description;

        /// <summary>
        /// Read and parse arguments.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        public CommandExecutionContext Read(string[] args)
        {
            var commandConfigFlags = _commandConfig.Flags;
            commandConfigFlags.Add(
                new Flag
                {
                    Name = HelpFlag,
                    Abr = HelpFlagAbr,
                    Description = HelpDescription,
                    DefaultValue = false
                }
            );
            
            var commandExecutionContext = new CommandInt(
                _nFlagsConfig.Dialect,
                _commandConfig.Commands,
                commandConfigFlags,
                _commandConfig.Parameters,
                _commandConfig.Options,
                InitDefaultCommandArgs(),
                _commandConfig.Execute,
                args
            ).Read();

            return commandExecutionContext.Args != null && commandExecutionContext.Args.Flags[HelpFlag]
                ? new CommandExecutionContext((commandArgs, output) => output(PrintHelp()), null) 
                : commandExecutionContext;
        }

        private string PrintHelp()
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
            private readonly Action<CommandArgs, Action<string>> _execute;

            public CommandInt(
                Dialect dialect, 
                List<Command> commands, 
                List<Flag> flags, 
                List<Parameter> parameters, 
                List<Option> options, 
                CommandArgs commandArgs, 
                Action<CommandArgs, Action<string>> execute,
                string[] args)
            {
                _dialect = dialect;
                _commands = commands;
                _flags = flags;
                _parameters = new Shifter<Parameter>(parameters.ToArray());
                _options = options;
                _execute = execute;
                _args = new Shifter<string>(args);
                _commandArgs = commandArgs;
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
                
                return new CommandExecutionContext(_execute, _commandArgs);
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