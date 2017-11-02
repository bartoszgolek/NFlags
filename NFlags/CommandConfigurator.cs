using System;
using System.Collections.Generic;

namespace NFlags
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandConfigurator
    {
        private readonly NFlagsConfig _nFlagsConfig;
        
        private readonly List<Parameter> _parameters = new List<Parameter>();

        private readonly List<Flag> _flags = new List<Flag>();

        private readonly List<Option> _options = new List<Option>();

        public CommandConfigurator(NFlagsConfig nFlagsConfig)
        {
            _nFlagsConfig = nFlagsConfig;
        }
        
        public CommandConfigurator RegisterOption(string name, string abr, string description, Action<string> handler)
        {
            _options.Add(new Option{ Name = name, Abr = abr, Description = description, Action = handler });

            return this;
        }

        public CommandConfigurator RegisterOption(string name, string description, Action<string> handler)
        {
            _options.Add(new Option{ Name = name, Description = description, Action = handler });

            return this;
        }

        public CommandConfigurator RegisterFlag(string name, string abr, string description, Action handler)
        {
            _flags.Add(new Flag{ Name = name, Abr = abr, Description = description, Action = handler });

            return this;
        }

        public CommandConfigurator RegisterFlag(string name, string description, Action handler)
        {
            _flags.Add(new Flag{ Name = name, Description = description, Action = handler });

            return this;
        }

        public CommandConfigurator RegisterParam(string name, string description, Action<string> handler)
        {
            _parameters.Add(new Parameter{ Name = name, Description = description, Action = handler });

            return this;
        }

        internal Command CreateCommand()
        {
            return new Command(
                _nFlagsConfig, 
                new CommandConfig(_flags, _parameters, _options)
            );
        }
    }
}