using System.Collections.Generic;
using System.Linq;
using NFlags.Utils;

namespace NFlags
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
                _commandConfig.Flags,
                new Shifter<Parameter>(_commandConfig.Parameters.ToArray()),
                _commandConfig.Options,
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

        private class CommandInt
        {
            private readonly Dialect _dialect;
            private readonly List<Flag> _flags;
            private readonly Shifter<Parameter> _parameters;
            private readonly List<Option> _options;
            private readonly Shifter<string> _args;

            public CommandInt(Dialect dialect, List<Flag> flags, Shifter<Parameter> parameters, List<Option> options,
                string[] args)
            {
                _dialect = dialect;
                _flags = flags;
                _parameters = parameters;
                _options = options;
                _args = new Shifter<string>(args);
            }

            public void Read()
            {
                while (_args.HasData())
                {
                    var arg = _args.Shift();
                    if (!ReadOpt(arg) && !ReadFlag(arg))
                        ReadParam(arg);
                }
            }

            private void ReadParam(string arg)
            {
                if (_parameters.HasData())
                    _parameters.Shift().Action(arg);
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

                opt.Action(
                    OptionReader.GetReader(_dialect.OptionValueMode).ReadValue(_args, arg)
                );

                return true;
            }

            private bool ReadFlag(string arg)
            {
                var flag = _flags.FirstOrDefault(
                    f => ArgMatcher.GetMatcher(_dialect).IsFlagMatching(f, arg)
                );

                if (flag == null)
                    return false;

                flag.Action();
                return true;
            }
        }
    }
}