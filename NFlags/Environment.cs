
using NFlags.Utils;

namespace NFlags
{
    /// <summary>
    /// Represents ability to write output.
    /// </summary>
    public static class Environment
    {
        /// <summary>
        /// Environment implementation to get from system environment.
        /// </summary>
        public static readonly IEnvironment System = new SystemEnvironment();
        
        /// <summary>
        /// Environment implementation to get from system environment with prefixed names.
        /// </summary>
        public static IEnvironment Prefixed(string prefix) => new PrefixedSystemEnvironment(prefix);
    }
}