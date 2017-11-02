using System.Collections.Generic;

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
        /// <param name="name">Command name</param>
        /// <param name="commands">List of registerd commands</param>
        /// <param name="flags">List of registerd flags</param>
        /// <param name="options">List of registerd options</param>
        /// <param name="parameters">List of registerd parameters</param>
        public CommandConfig(
            string name,
            List<Command> commands,
            List<Flag> flags, 
            List<Option> options, 
            List<Parameter> parameters)
        {
            Name = name;
            Flags = flags;
            Options = options;
            Parameters = parameters;
            Commands = commands;
        }

        /// <summary>
        /// List of registerd commands
        /// </summary>
        public List<Command> Commands { get; }

        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; }

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
    }
}