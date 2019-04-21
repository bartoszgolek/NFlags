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
        private readonly CliConfig _cliConfig;
        private readonly List<CommandConfigurator> _commands = new List<CommandConfigurator>();
        private CommandConfigurator _defaultCommand;
        private readonly List<Parameter> _parameters = new List<Parameter>();
        private readonly List<PrefixedDefaultValueArgument> _options = new List<PrefixedDefaultValueArgument>();
        private ParameterSeries _paramSeries;
        private bool _printHelpOnExecute;
        private Func<CommandArgs, IOutput, int> _execute;

        private CommandConfigurator(string name, List<string> parents, string description, CliConfig cliConfig)
        {
            Name = name;
            Description = description;
            _parents = parents;
            _cliConfig = cliConfig;
        }

        internal CommandConfigurator(string name, string description, CliConfig cliConfig)
        {
            Name = name;
            Description = description;
            _parents = new List<string>();
            _cliConfig = cliConfig;
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
        /// Sets function to execute when command is called.
        /// </summary>
        /// <param name="execute">Function invoked when executing command. Returns exit code.</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator SetExecute(Func<CommandArgs, IOutput, int> execute)
        {
            _execute = execute;

            return this;
        }

        /// <summary>
        /// Sets function to execute when command is called
        /// </summary>
        /// <param name="execute">Action invoked when executing command.</param>
        /// <returns>Self instance</returns>
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
        /// Configures command to print help when executed. Useful when command is a bucket to group sub commands and not require implementation.
        /// </summary>
        /// <returns>Self instance</returns>
        public CommandConfigurator PrintHelpOnExecute()
        {
            _printHelpOnExecute = true;

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
            RegisterNewCommand(name, description, configureCommand);

            return this;
        }

        /// <summary>
        /// Register sub command for the command and set it as default.
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Command description for help.</param>
        /// <param name="configureCommand">Command configuration callback</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterDefaultCommand(string name, string description,
            Action<CommandConfigurator> configureCommand)
        {
            if (_defaultCommand != null)
                throw new TooManyDefaultCommandsException();

            var commandConfigurator = RegisterNewCommand(name, description, configureCommand);
            _defaultCommand = commandConfigurator;

            return this;
        }

        private CommandConfigurator RegisterNewCommand(string name, string description, Action<CommandConfigurator> configureCommand)
        {
            var commandConfigurator = new CommandConfigurator(name, GetCommandParents(), description, _cliConfig);
            configureCommand(commandConfigurator);
            _commands.Add(commandConfigurator);

            return commandConfigurator;
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
        /// <param name="buildFlag">Action to configure flag using builder</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterFlag(Action<FlagBuilder> buildFlag)
        {
            var flagBuilder = new FlagBuilder();
            buildFlag(flagBuilder);

            return RegisterFlagInstance(flagBuilder.Build());
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
            return RegisterFlagInstance(
                new Flag {Name = name, Description = description, DefaultValue = defaultValue}
            );
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
            return RegisterFlagInstance(
                new Flag {Name = name, Abr = abr, Description = description, DefaultValue = defaultValue}
            );
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
            return RegisterFlagInstance(
                new Flag
                {
                    Name = name,
                    Abr = abr,
                    Description = description,
                    DefaultValue = defaultValue,
                    IsPersistent = true
                }
            );
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
            return RegisterFlagInstance(
                new Flag
                {
                    Name = name,
                    Description = description,
                    DefaultValue = defaultValue,
                    IsPersistent = true
                }
            );
        }

        internal CommandConfigurator RegisterFlagInstance(Flag flag)
        {
            _options.Add(flag);

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
            return RegisterOptionInstance<T>(
                new Option {Name = name, Abr = abr, Description = description, DefaultValue = defaultValue, ValueType = typeof(T)}
            );
        }

        /// <summary>
        /// Register option for the command
        /// </summary>
        /// <param name="buildOption">Action to configure option using builder</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterOption<T>(Action<OptionBuilder<T>> buildOption)
        {
            var optionBuilder = new OptionBuilder<T>();
            buildOption(optionBuilder);

            return RegisterOptionInstance<T>(optionBuilder.Build());
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
            return RegisterOptionInstance<T>(
                new Option {Name = name, Description = description, DefaultValue = defaultValue, ValueType = typeof(T)}
            );
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
            return RegisterOptionInstance<T>(new Option
            {
                Name = name,
                Description = description,
                DefaultValue = defaultValue,
                IsPersistent = true,
                ValueType = typeof(T)
            });
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
            return RegisterOptionInstance<T>(
                new Option
                {
                    Name = name,
                    Abr = abr,
                    Description = description,
                    DefaultValue = defaultValue,
                    IsPersistent = true,
                    ValueType = typeof(T)
                }
            );
        }

        internal CommandConfigurator RegisterOptionInstance<T>(Option option)
        {
            CheckConverterIsRegistered(typeof(T));
            _options.Add(option);

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
            return RegisterParameterInstance<T>(new Parameter {Name = name, Description = description, DefaultValue = defaultValue, ValueType = typeof(T)});
        }

        /// <summary>
        /// Register parameter for the command
        /// </summary>
        /// <param name="buildParameter">Action to configure flag using builder</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterParameter<T>(Action<ParameterBuilder<T>> buildParameter)
        {
            var parameterBuilder = new ParameterBuilder<T>();
            buildParameter(parameterBuilder);
            return RegisterParameterInstance<T>(parameterBuilder.Build());
        }

        internal CommandConfigurator RegisterParameterInstance<T>(Parameter parameter)
        {
            CheckConverterIsRegistered(typeof(T));
            _parameters.Add(parameter);

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
            var parameterSeries = new ParameterSeries {Name = name, Description = description, ValueType = typeof(T)};

            return RegisterParameterSeriesInstance<T>(parameterSeries);
        }

        /// <summary>
        /// Register parameter series for the command
        /// </summary>
        /// <param name="buildParameterSeries">Action to configure parameter series using builder</param>
        /// <returns>Self instance</returns>
        public CommandConfigurator RegisterParameterSeries<T>(Action<ParameterSeriesBuilder<T>> buildParameterSeries)
        {
            var parameterSeriesBuilder = new ParameterSeriesBuilder<T>();
            buildParameterSeries(parameterSeriesBuilder);

            return RegisterParameterSeriesInstance<T>(parameterSeriesBuilder.Build());
        }

        internal CommandConfigurator RegisterParameterSeriesInstance<T>(ParameterSeries parameterSeries)
        {
            CheckConverterIsRegistered(typeof(T));
            _paramSeries = parameterSeries;
            return this;
        }

        internal Command CreateCommand(CommandConfig parentConfig = null)
        {
            return new Command(
                new CommandConfig(
                    _cliConfig,
                    Name,
                    _printHelpOnExecute,
                    _parents,
                    _commands,
                    _defaultCommand,
                    GetCommandOptions(parentConfig),
                    _parameters,
                    _paramSeries,
                    _execute
                )
            );
        }

        private List<PrefixedDefaultValueArgument> GetCommandOptions(CommandConfig parentConfig)
        {
            var commandOptions = new List<PrefixedDefaultValueArgument>();
            commandOptions.AddRange(_options);
            if (parentConfig != null)
                commandOptions.AddRange(parentConfig.Options.Where(f => f.IsPersistent));
            return commandOptions;
        }

        private void CheckConverterIsRegistered(Type type)
        {
            var typeToCheck = type.IsArray ? type.GetElementType() : type;

            if (_cliConfig.ArgumentConverters.Any(argumentConverter => argumentConverter.CanConvert(typeToCheck)))
                return;

            throw new MissingConverterException(type);
        }
    }
}