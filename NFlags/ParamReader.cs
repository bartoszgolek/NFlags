﻿using System.Collections.Generic;
using System.Linq;
using NFlags.Utils;

namespace NFlags
{
    public class ParamReader
    {
        private readonly string _name;
        private readonly string _description;
        private readonly Dialect _dialect;
        private readonly List<Flag> _flags;
        private readonly List<Arg> _parameters;
        private readonly List<Option> _options;

        public ParamReader(
            string name,
            string description,
            Dialect dialect, 
            List<Arg> parameters, 
            List<Flag> flags, 
            List<Option> options)
        {
            _name = name;
            _description = description;
            _dialect = dialect;
            _parameters = parameters;
            _flags = flags;
            _options = options;
        }

        public void Read(string[] args)
        {
            new ParamReaderInt(
                _dialect,
                _flags,
                new Shifter<Arg>(_parameters.ToArray()),
                _options,
                args
            ).Read();
        }

        public string PrintHelp()
        {
            return new HelpPrinter(_name, _description, _dialect, _flags, _parameters, _options).Print();
        }

        private class ParamReaderInt
        {
            private readonly Dialect _dialect;
            private readonly List<Flag> _flags;
            private readonly Shifter<Arg> _parameters;
            private readonly List<Option> _options;
            private readonly Shifter<string> _args;

            public ParamReaderInt(Dialect dialect, List<Flag> flags, Shifter<Arg> parameters, List<Option> options,
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
                    throw new TooManyArgumentsException(arg);
            }

            private bool ReadOpt(string arg)
            {
                var opt = _options.FirstOrDefault(
                    option => ArgMatcher.GetMatcher(_dialect).IsOptionMatching(option, arg)
                );

                if (opt == null)
                    return false;

                opt.Action(
                    OptionReader.GetReader(_dialect.OptionSeparator).ReadValue(_args, arg)
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