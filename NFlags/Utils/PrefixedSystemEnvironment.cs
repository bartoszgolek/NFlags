namespace NFlags.Utils
{
    /// <inheritdoc />
    /// <summary>
    /// Provides environment variables with prefix 
    /// </summary>
    public class PrefixedSystemEnvironment : IEnvironment
    {
        private readonly string _prefix;

        /// <summary>
        /// Create new instance. 
        /// </summary>
        /// <param name="prefix">Prefix to add when requesting for environment variable</param>
        public PrefixedSystemEnvironment(string prefix)
        {
            _prefix = prefix;
        }

        /// <inheritdoc />
        public string Get(string name)
        {
            return System.Environment.GetEnvironmentVariable(_prefix + name);
        }
    }
}