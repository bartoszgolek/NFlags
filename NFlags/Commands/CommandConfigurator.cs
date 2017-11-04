using System;
using System.Collections.Generic;
using NFlags.Arguments;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents Command configuration ability. 
    /// </summary>
    public class CommandConfigurator
    {
        private readonly string _name;
        private readonly List<string> _parents;
        private readonly string _description;
        private readonly NFlagsConfig _nFlagsConfig;
        private readonly List<Command> _commands = new List<Command>();
        private readonly List<Parameter> _parameters = new List<Parameter>();
        private readonly List<Flag> _flags = new List<Flag>();
        private readonly List<Option> _options = new List<Option>();
        private Action<CommandArgs, Action<string>> _execute;

        private CommandConfigurator(string name, List<string> parents, string description, NFlagsConfig nFlagsConfig)
        {
            _name = name;
            _parents = parents;
            _nFlagsConfig = nFlagsConfig;
            _description = description;
        }

        internal CommandConfigurator(string name, string description, NFlagsConfig nFlagsConfig)
        {
            _name = name;
            _parents = new List<string>();
            _nFlagsConfig = nFlagsConfig;
            _description = description;
        }

        /// <summary>
        /// Sets function to execute when command is called
        /// </summary>
        /// <param name="execute">Function to execute when command is called</param>
        /// <returns></returns>
        public CommandConfigurator SetExecute(Action<CommandArgs, Action<string>> execute)
        {
            _execute = execute;

            return this;
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
        /// Register sub command for the command
        /// </summary>
        /// <param name="name">Subcommand name</param>
        /// <param name="description">Subcommand description for help.</param>
        /// <param name="configureCommand">Command configuration callback</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterSubcommand(string name, string description, Action<CommandConfigurator> configureCommand)
        {
            var commandConfigurator = new CommandConfigurator(name, GetSubcommandParents(), description, _nFlagsConfig);
            configureCommand(commandConfigurator);
            _commands.Add(commandConfigurator.CreateCommand());

            return this;
        }

        private List<string> GetSubcommandParents()
        {
            var subcommandParents = new List<string>(_parents.Count + 1);
            subcommandParents.AddRange(_parents);
            subcommandParents.Add(_name);
            return subcommandParents;
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
                new CommandConfig(_name, _parents, _description, _commands, _flags, _options, _parameters, _execute)
            );
        }
    }
}