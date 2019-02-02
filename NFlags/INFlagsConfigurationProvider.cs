using System.Collections.Generic;
using NFlags.TypeConverters;

namespace NFlags
{
    /// <summary>
    /// Interface to provide NFlags configuration using any implementation.  
    /// </summary>
    public interface INFlagsConfigurationProvider
    {
        /// <summary>
        /// Name of application
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of application
        /// </summary>
        string Description { get; }

        /// <summary>
        /// NFlags dialect for parsing arguments
        /// </summary>
        Dialect Dialect { get; }

        /// <summary>
        /// NFlags output. Console if null.
        /// </summary>
        IOutput Output { get; }

        /// <summary>
        /// Determines if exception handling should be enabled. If False NFlags will throw exceptions instead of return exit code; 
        /// </summary>
        bool IsExceptionHandlingEnabled { get; }
        
        /// <summary>
        /// Converters used to convert values into particular types.
        /// </summary>
        IEnumerable<IArgumentConverter> Converters { get; }
    }
}