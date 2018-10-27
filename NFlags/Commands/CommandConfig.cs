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
        /// <param name="nFlagsConfig">NFlags config</param>
        /// <param name="name">Command name</param>
        /// <param name="parents">Command parents list</param>
        /// <param name="commands">List of registered commands</param>
        /// <param name="flags">List of registered flags</param>
        /// <param name="options">List of registered options</param>
        /// <param name="parameters">List of registered parameters</param>
        /// <param name="parameterSeries">Registered parameter series</param>
        /// <param name="execute">Function to execute when command is called</param>
        public CommandConfig(
            NFlagsConfig nFlagsConfig,
            string name,
            List<string> parents,
            List<CommandConfigurator> commands,
            List<Flag> flags,
            List<Option> options,
            List<Parameter> parameters,
            ParameterSeries parameterSeries,
            Func<CommandArgs, IOutput, int> execute)
        {
            NFlagsConfig = nFlagsConfig;
            Name = name;
            Parents = parents;
            Flags = flags;
            Options = options;
            Parameters = parameters;
            ParameterSeries = parameterSeries;
            Execute = execute;
            Commands = commands;
        }

        /// <summary>
        /// List of registered commands
        /// </summary>
        public List<CommandConfigurator> Commands { get; }

        /// <summary>
        /// NFlags config
        /// </summary>
        public NFlagsConfig NFlagsConfig { get; }

        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Command parents list
        /// </summary>
        public List<string> Parents { get; }

        /// <summary>
        /// List of registered flags
        /// </summary>
        public List<Flag> Flags { get; }

        /// <summary>
        /// List of registered options
        /// </summary>
        public List<Option> Options { get; }

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
    }
}