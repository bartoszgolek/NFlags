using System;
using System.Collections.Generic;

namespace NFlags
{
    public class ParamReaderConfigurator
    {
        private string _name = AppDomain.CurrentDomain.FriendlyName;
        
        private string _description = "";
        
        private Dialect _dialect = Dialect.Win;

        private readonly List<Parameter> _parameters = new List<Parameter>();

        private readonly List<Flag> _flags = new List<Flag>();

        private readonly List<Option> _options = new List<Option>();

        public ParamReaderConfigurator SetName(string name)
        {
            _name = name;

            return this;
        }

        public ParamReaderConfigurator SetDescription(string description)
        {
            _description = description;

            return this;
        }

        public ParamReaderConfigurator SetDialect(Dialect dialect)
        {
            _dialect = dialect;

            return this;
        }

        public ParamReaderConfigurator RegisterOption(string name, string abr, string description, Action<string> handler)
        {
            _options.Add(new Option{ Name = name, Abr = abr, Description = description, Action = handler });

            return this;
        }

        public ParamReaderConfigurator RegisterOption(string name, string description, Action<string> handler)
        {
            _options.Add(new Option{ Name = name, Description = description, Action = handler });

            return this;
        }

        public ParamReaderConfigurator RegisterFlag(string name, string abr, string description, Action handler)
        {
            _flags.Add(new Flag{ Name = name, Abr = abr, Description = description, Action = handler });

            return this;
        }

        public ParamReaderConfigurator RegisterFlag(string name, string description, Action handler)
        {
            _flags.Add(new Flag{ Name = name, Description = description, Action = handler });

            return this;
        }

        public ParamReaderConfigurator RegisterParam(string name, string description, Action<string> handler)
        {
            _parameters.Add(new Parameter{ Name = name, Description = description, Action = handler });

            return this;
        }

        internal ParamReader CreateParamReader()
        {
            return new ParamReader(_name, _description, _dialect, _parameters, _flags, _options);
        }
    }
}