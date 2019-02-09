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
}