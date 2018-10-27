using System.Collections.Generic;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents parsed command arguments.
    /// </summary>
    public class CommandArgs
    {
        /// <summary>
        /// Creates new instance of CommandArgs
        /// </summary>
        public CommandArgs()
        {
            Flags = new Dictionary<string, bool>();
            Options = new Dictionary<string, object>();
            Parameters = new Dictionary<string, object>();
            ParameterSeries = new List<object>();
        }

        /// <summary>
        /// All registered flags with values
        /// </summary>
        public Dictionary<string, bool> Flags { get; }
        
        /// <summary>
        /// All registered options with values
        /// </summary>
        public Dictionary<string, object> Options { get; }
        
        /// <summary>
        /// All registered parameters with values
        /// </summary>
        public Dictionary<string, object> Parameters { get; }
        
        /// <summary>
        /// Registered parameter series with values
        /// </summary>
        public List<object> ParameterSeries { get; }
    }
}