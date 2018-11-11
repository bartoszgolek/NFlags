using System;
using System.Collections.Generic;
using System.Linq;
using NFlags.Arguments;
using NFlags.TypeConverters;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents Command configuration ability.
    /// </summary>
    public class CommandConfigurator
    {
        private readonly List<string> _parents;
        private readonly NFlagsConfig _nFlagsConfig;
        private readonly List<CommandConfigurator> _commands = new List<CommandConfigurator>();
        private readonly List<Parameter> _parameters = new List<Parameter>();
        private readonly List<Flag> _flags = new List<Flag>();
        private readonly List<Option> _options = new List<Option>();
        private ParameterSeries _paramSeries;
        private Func<CommandArgs, IOutput, int> _execute;

        private CommandConfigurator(string name, List<string> parents, string description, NFlagsConfig nFlagsConfig)
        {
            Name = name;
            Description = description;
            _parents = parents;
            _nFlagsConfig = nFlagsConfig;
        }

        internal CommandConfigurator(string name, string description, NFlagsConfig nFlagsConfig)
        {
            Name = name;
            Description = description;
            _parents = new List<string>();
            _nFlagsConfig = nFlagsConfig;
        }

        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Command description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Sets function to execute when command is called. Returns exit code.
        /// </summary>
        /// <param name="execute">Interface to print to output</param>
        /// <returns>Command configurator</returns>
        public CommandConfigurator SetExecute(Func<CommandArgs, IOutput, int> execute)
        {
            _execute = execute;

            return this;
        }

        /// <summary>
        /// Sets function to execute when command is called
        /// </summary>
        /// <param name="execute">Interface to print to output</param>
        /// <returns>Command configurator</returns>
        public CommandConfigurator SetExecute(Action<CommandArgs, IOutput> execute)
        {
            _execute = (args, output) =>
            {
                execute(args, output);

                return 0;
            };

            return this;
        }

        /// <summary>
        /// Register sub command for the command
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Command description for help.</param>
        /// <param name="configureCommand">Command configuration callback</param>
        /// <returns>Self instance</returns>
        [Obsolete("RegisterSubcommand method is obsolete. Use RegisterCommand instead.")]
        public CommandConfigurator RegisterSubcommand(string name, string description,
            Action<CommandConfigurator> configureCommand)
        {
            return RegisterCommand(name, description, configureCommand);
        }

        /// <summary>
        /// Register sub command for the command
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Command description for help.</param>
        /// <param name="configureCommand">Command configuration callback</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterCommand(string name, string description,
            Action<CommandConfigurator> configureCommand)
        {
            var commandConfigurator = new CommandConfigurator(name, GetCommandParents(), description, _nFlagsConfig);
            configureCommand(commandConfigurator);
            _commands.Add(commandConfigurator);

            return this;
        }

        private List<string> GetCommandParents()
        {
            var commandParents = new List<string>(_parents.Count + 1);
            commandParents.AddRange(_parents);
            commandParents.Add(Name);
            return commandParents;
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
            _flags.Add(new Flag {Name = name, Description = description, DefaultValue = defaultValue});

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
            _flags.Add(new Flag {Name = name, Abr = abr, Description = description, DefaultValue = defaultValue});

            return this;
        }

        /// <summary>
        /// Register flag for the command and sub commands
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <param name="abr">Flag shorthand</param>
        /// <param name="description">Flag description for help.</param>
        /// <param name="defaultValue">Default flag value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterPersistentFlag(string name, string abr, string description,
            bool defaultValue)
        {
            _flags.Add(new Flag
            {
                Name = name,
                Abr = abr,
                Description = description,
                DefaultValue = defaultValue,
                IsPersistent = true
            });

            return this;
        }

        /// <summary>
        /// Register flag for the command and sub commands
        /// </summary>
        /// <param name="name">Flag name</param>
        /// <param name="description">Flag description for help.</param>
        /// <param name="defaultValue">Default flag value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterPersistentFlag(string name, string description, bool defaultValue)
        {
            _flags.Add(new Flag
            {
                Name = name,
                Description = description,
                DefaultValue = defaultValue,
                IsPersistent = true
            });

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
        public CommandConfigurator RegisterOption<T>(string name, string abr, string description, T defaultValue)
        {
            CheckConverterIsRegistered(typeof(T));
            _options.Add(new Option {Name = name, Abr = abr, Description = description, DefaultValue = defaultValue, ValueType = typeof(T)});

            return this;
        }

        /// <summary>
        /// Register option for the command
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="description">Option description for help.</param>
        /// <param name="defaultValue">Default option value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterOption<T>(string name, string description, T defaultValue)
        {
            CheckConverterIsRegistered(typeof(T));
            _options.Add(new Option {Name = name, Description = description, DefaultValue = defaultValue, ValueType = typeof(T)});

            return this;
        }

        /// <summary>
        /// Register option for the command and sub commands
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="description">Option description for help.</param>
        /// <param name="defaultValue">Default option value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterPersistentOption<T>(string name, string description, T defaultValue)
        {
            CheckConverterIsRegistered(typeof(T));
            _options.Add(new Option
            {
                Name = name,
                Description = description,
                DefaultValue = defaultValue,
                IsPersistent = true,
                ValueType = typeof(T),
            });

            return this;
        }

        /// <summary>
        /// Register option for the command and sub commands
        /// </summary>
        /// <param name="name">Option name</param>
        /// <param name="abr">Option shorthand</param>
        /// <param name="description">Option description for help.</param>
        /// <param name="defaultValue">Default option value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterPersistentOption<T>(string name, string abr, string description, T defaultValue)
        {
            CheckConverterIsRegistered(typeof(T));
            _options.Add(new Option
            {
                Name = name,
                Abr = abr,
                Description = description,
                DefaultValue = defaultValue,
                IsPersistent = true
            });

            return this;
        }

        /// <summary>
        /// Register parameter for the command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="description">Parameter description for help.</param>
        /// <param name="defaultValue">Default parameter value.</param>
        /// <returns>Self instance</returns>
        [Obsolete("RegisterParam method is obsolete. Use RegisterParameter instead.")]
        public CommandConfigurator RegisterParam<T>(string name, string description, T defaultValue)
        {
            return RegisterParameter(name, description, defaultValue);
        }

        /// <summary>
        /// Register parameter for the command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="description">Parameter description for help.</param>
        /// <param name="defaultValue">Default parameter value.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterParameter<T>(string name, string description, T defaultValue)
        {
            CheckConverterIsRegistered(typeof(T));
            _parameters.Add(new Parameter {Name = name, Description = description, DefaultValue = defaultValue, ValueType = typeof(T)});

            return this;
        }

        /// <summary>
        /// Register parameter series for the command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="description">Parameter description for help.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterParameterSeries(string name, string description)
        {
            return RegisterParameterSeries<string>(name, description);
        }

        /// <summary>
        /// Register parameter series for the command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="description">Parameter description for help.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterParameterSeries<T>(string name, string description)
        {
            CheckConverterIsRegistered(typeof(T));
            _paramSeries = new ParameterSeries {Name = name, Description = description, ValueType = typeof(T)};

            return this;
        }

        internal Command CreateCommand(CommandConfig parentConfig = null)
        {
            return new Command(
                new CommandConfig(
                    _nFlagsConfig,
                    Name,
                    _parents,
                    _commands,
                    GetCommandFlags(parentConfig),
                    GetCommandOptions(parentConfig),
                    _parameters,
                    _paramSeries,
                    _execute
                )
            );
        }

        private List<Flag> GetCommandFlags(CommandConfig parentConfig)
        {
            var commandFlags = new List<Flag>();
            commandFlags.AddRange(_flags);
            if (parentConfig != null)
                commandFlags.AddRange(parentConfig.Flags.Where(f => f.IsPersistent));
            return commandFlags;
        }

        private List<Option> GetCommandOptions(CommandConfig parentConfig)
        {
            var commandOptions = new List<Option>();
            commandOptions.AddRange(_options);
            if (parentConfig != null)
                commandOptions.AddRange(parentConfig.Options.Where(f => f.IsPersistent));
            return commandOptions;
        }

        private void CheckConverterIsRegistered(Type type)
        {
            if (_nFlagsConfig.ArgumentConverters.Any(argumentConverter => argumentConverter.CanConvert(type)))
                return;

            throw new MissingConverterException(type);
        }
    }
}