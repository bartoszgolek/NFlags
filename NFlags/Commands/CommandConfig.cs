using System;
using System.Collections.Generic;
using NFlags.Arguments;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents configuration of command.
    /// </summary>
    public class CommandConfig
    {
        /// <summary>
        /// Creates new instance of command configuration.
        /// </summary>
        /// <param name="cliConfig">NFlags Cli config</param>
        /// <param name="name">Command name</param>
        /// <param name="printHelpOnExecute">If True command will print help instead of running execute method</param>
        /// <param name="parents">Command parents list</param>
        /// <param name="commands">List of registered commands</param>
        /// <param name="defaultCommand">Default command to run where subcommand is not defined in params</param>
        /// <param name="options">List of registered options</param>
        /// <param name="parameters">List of registered parameters</param>
        /// <param name="parameterSeries">Registered parameter series</param>
        /// <param name="execute">Function to execute when command is called</param>
        public CommandConfig(
            CliConfig cliConfig,
            string name,
            bool printHelpOnExecute,
            List<string> parents,
            List<CommandConfigurator> commands,
            CommandConfigurator defaultCommand,
            List<PrefixedDefaultValueArgument> options,
            List<Parameter> parameters,
            ParameterSeries parameterSeries,
            Func<CommandArgs, IOutput, int> execute)
        {
            CliConfig = cliConfig;
            Name = name;
            Parents = parents;
            Options = options;
            Parameters = parameters;
            ParameterSeries = parameterSeries;
            Execute = execute;
            PrintHelpOnExecute = printHelpOnExecute;
            DefaultCommand = defaultCommand;
            Commands = commands;
        }

        /// <summary>
        /// List of registered commands
        /// </summary>
        public List<CommandConfigurator> Commands { get; }

        /// <summary>
        /// NFlags Cli config
        /// </summary>
        public CliConfig CliConfig { get; }

        /// <summary>
        /// NFlags config
        /// </summary>
        [Obsolete("NFlags property is obsolete. Use CliCOnfig instead.")]
        public CliConfig NFlagsConfig { get; }

        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// If True command will print help instead of running execute method
        /// </summary>
        public bool PrintHelpOnExecute { get; }

        /// <summary>
        /// Command parents list
        /// </summary>
        public List<string> Parents { get; }

        /// <summary>
        /// List of registered options
        /// </summary>
        public List<PrefixedDefaultValueArgument> Options { get; }

        /// <summary>
        /// List of registered parameters
        /// </summary>
        public List<Parameter> Parameters { get; }

        /// <summary>
        /// Registered parameter series
        /// </summary>
        public ParameterSeries ParameterSeries { get; }

        /// <summary>
        /// Function to execute when command is called.
        /// </summary>
        public Func<CommandArgs, IOutput, int> Execute { get; }

        /// <summary>
        /// Default command to run where subcommand is not defined in params
        /// </summary>
        public CommandConfigurator DefaultCommand { get; }
    }
}