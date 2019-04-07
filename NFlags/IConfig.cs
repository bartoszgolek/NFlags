namespace NFlags
{
    /// <summary>
    /// Provides config values for arguments
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Get value from config. 
        /// </summary>
        /// <param name="path">Config path</param>
        /// <returns>Config value</returns>
        string Get(string path);
    }

    /// <summary>
    /// Provides config values for arguments
    /// </summary>
    public interface IGenericConfig
    {
        /// <summary>
        /// Check if config contains value. 
        /// </summary>
        /// <param name="path">Config path</param>
        /// <returns>True if configurations contains value for path, False otherwise</returns>
        bool Has(string path);

        /// <summary>
        /// Get value from config. 
        /// </summary>
        /// <param name="path">Config path</param>
        /// <returns>Config value</returns>
        T Get<T>(string path);
    }
}