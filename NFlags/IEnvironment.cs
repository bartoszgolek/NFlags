namespace NFlags
{
    /// <summary>
    /// Provides environment variables
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Get value of variable. 
        /// </summary>
        /// <param name="name">Name of environment variable</param>
        /// <returns>Variable value</returns>
        string Get(string name);
    }
}