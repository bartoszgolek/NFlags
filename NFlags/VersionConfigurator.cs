namespace NFlags
{
    /// <summary>
    /// Represents NFlags version configuration ability.
    /// </summary>
    public class VersionConfigurator
    {
        private const string DefaultVersionFlag = "version";
        private const string DefaultVersionFlagAbr = "v";
        private const string DefaultVersionDescription = "Prints application version";

        private bool _enabled;
        private string _description = DefaultVersionDescription;
        private string _flag = DefaultVersionFlag;
        private string _abr = DefaultVersionFlagAbr;

        /// <summary>
        /// Enables version option
        /// </summary>
        /// <returns>Self instance</returns>
        public VersionConfigurator Enable()
        {
            _enabled = true;

            return this;
        }

        /// <summary>
        /// Sets help flag for print help option.
        /// </summary>
        /// <param name="flag">Help flag for print help option.</param>
        /// <returns>Self instance</returns>
        public VersionConfigurator SetOptionFlag(string flag)
        {
            _flag = flag;

            return this;
        }

        /// <summary>
        /// Help flag abbreviation for print help option.
        /// </summary>
        /// <param name="abr">Help flag abbreviation for print help option.</param>
        /// <returns>Self instance</returns>
        public VersionConfigurator SetOptionAbr(string abr)
        {
            _abr = abr;

            return this;
        }

        /// <summary>
        /// Sets text for help option printed in help.
        /// </summary>
        /// <param name="description">Text for help option printed in help.</param>
        /// <returns>Self instance</returns>
        public VersionConfigurator SetOptionDescription(string description)
        {
            _description = description;

            return this;
        }

        internal VersionConfig GetVersionConfig()
        {
            return new VersionConfig(_enabled, _flag, _abr, _description);
        }
    }
}