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
        /// <param name="description">Command description</param>
        /// <param name="commands">List of registerd commands</param>
        /// <param name="flags">List of registerd flags</param>
        /// <param name="options">List of registerd options</param>
        /// <param name="parameters">List of registerd parameters</param>
        /// <param name="execute">Function to execute when command is called</param>
        public CommandConfig(
            NFlagsConfig nFlagsConfig,
            string name,
            List<string> parents,
            string description,
            List<CommandConfigurator> commands,
            List<Flag> flags,
            List<Option> options,
            List<Parameter> parameters,
            Action<CommandArgs,
            Action<string>> execute)
        {
            NFlagsConfig = nFlagsConfig;
            Name = name;
            Parents = parents;
            Description = description;
            Flags = flags;
            Options = options;
            Parameters = parameters;
            Execute = execute;
            Commands = commands;
        }

        /// <summary>
        /// List of registerd commands
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
        /// Command description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// List of registerd flags
        /// </summary>
        public List<Flag> Flags { get; }

        /// <summary>
        /// List of registerd options
        /// </summary>
        public List<Option> Options { get; }

        /// <summary>
        /// List of registerd parameters
        /// </summary>
        public List<Parameter> Parameters { get; }

        /// <summary>
        /// Function to execute when command is called.
        /// </summary>
        public Action<CommandArgs, Action<string>> Execute { get; }
    }
}