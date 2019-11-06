namespace NFlags
{
    /// <summary>
    /// Configuration of NFlags version option
    /// </summary>
    public class VersionConfig
    {
        /// <summary>
        /// Creates new instance of VersionConfig
        /// </summary>
        /// <param name="enabled">Determine if version option should be enabled</param>
        /// <param name="flag">Version flag for print help option</param>
        /// <param name="abr">Version flag abbreviation for print help option</param>
        /// <param name="description">Text for help version printed in help</param>
        public VersionConfig(bool enabled, string flag, string abr, string description)
        {
            Enabled = enabled;
            Flag = flag;
            Abr = abr;
            Description = description;
        }

        /// <summary>
        /// Application name
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// Help flag for print help option
        /// </summary>
        public string Flag { get; }

        /// <summary>
        /// Help flag abbreviation for print help option
        /// </summary>
        public string Abr { get; }

        /// <summary>
        /// Text for help option printed in help
        /// </summary>
        public string Description { get; }
    }
}