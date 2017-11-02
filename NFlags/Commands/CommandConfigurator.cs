using System;
using System.Collections.Generic;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents Command configuration ability. 
    /// </summary>
    public class CommandConfigurator
    {
        private readonly string _name;
        private readonly NFlagsConfig _nFlagsConfig;
        private readonly List<Command> _commands = new List<Command>();
        private readonly List<Parameter> _parameters = new List<Parameter>();
        private readonly List<Flag> _flags = new List<Flag>();
        private readonly List<Option> _options = new List<Option>();

        /// <summary>
        /// Creates new instance of CommandConfigurator
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="nFlagsConfig">NFlags configuration</param>
        public CommandConfigurator(string name, NFlagsConfig nFlagsConfig)
        {
            _name = name;
            _nFlagsConfig = nFlagsConfig;
        }

        /// <summary>
        /// Register flag for the command
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <param name="abr">Flag shorthand</param>
        /// <param name="description">Flag description for help.</param>
        /// <param name="defaultValue">Default flag value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterFlag(string name, string abr, string description, bool defaultValue)
        {
            _flags.Add(new Flag{ Name = name, Abr = abr, Description = description, DefaultValue = defaultValue });

            return this;
        }

        /// <summary>
        /// Register flag for the command
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <param name="description">Flag description for help.</param>
        /// <param name="defaultValue">Default flag value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterFlag(string name, string description, bool defaultValue)
        {
            _flags.Add(new Flag{ Name = name, Description = description, DefaultValue = defaultValue});

            return this;
        }
        
        /// <summary>
        /// Register option for the command
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="abr">Option shorthand</param>
        /// <param name="description">Option description for help.</param>
        /// <param name="defaultValue">Default option value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterOption(string name, string abr, string description, string defaultValue)
        {
            _options.Add(new Option{ Name = name, Abr = abr, Description = description, DefaultValue = defaultValue});

            return this;
        }

        /// <summary>
        /// Register option for the command
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="description">Option description for help.</param>
        /// <param name="defaultValue">Default option value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterOption(string name, string description, string defaultValue)
        {
            _options.Add(new Option{ Name = name, Description = description, DefaultValue = defaultValue});

            return this;
        }

        /// <summary>
        /// Register parameter for the command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="description">Parameter description for help.</param>
        /// <param name="defaultValue">Default parameter value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterParam(string name, string description, string defaultValue)
        {
            _parameters.Add(new Parameter{ Name = name, Description = description, DefaultValue = defaultValue });

            return this;
        }

        internal Command CreateCommand()
        {
            return new Command(
                _nFlagsConfig, 
                new CommandConfig(_name, _commands, _flags, _options, _parameters)
            );
        }
    }
}