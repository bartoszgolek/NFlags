using System.Collections.Generic;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents parsed command arguments.
    /// </summary>
    public class CommandArgs
    {
        public CommandArgs()
        {
            Flags = new Dictionary<string, bool>();
            Options = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// All registered flags with values
        /// </summary>
        public Dictionary<string, bool> Flags { get; }
        
        /// <summary>
        /// All registered options with values
        /// </summary>
        public Dictionary<string, string> Options { get; }
        
        /// <summary>
        /// All registered parameters with values
        /// </summary>
        public Dictionary<string, string> Parameters { get; }
    }
}