namespace NFlags
{
    /// <summary>
    /// Represents NFlags configuration ability.
    /// </summary>
    public class HelpConfigurator
    {
        private const string DefaultHelpFlag = "help";
        private const string DefaultHelpFlagAbr = "h";
        private const string DefaultHelpDescription = "Prints this help";

        private IHelpPrinter _printer = HelpPrinter.Default;
        private string _description = DefaultHelpDescription;
        private string _flag = DefaultHelpFlag;
        private string _abr = DefaultHelpFlagAbr;

        /// <summary>
        /// Sets help flag for print help option.
        /// </summary>
        /// <param name="flag">Help flag for print help option.</param>
        /// <returns>Self instance</returns>
        public HelpConfigurator SetOptionFlag(string flag)
        {
            _flag = flag;

            return this;
        }

        /// <summary>
        /// Help flag abbreviation for print help option.
        /// </summary>
        /// <param name="abr">Help flag abbreviation for print help option.</param>
        /// <returns>Self instance</returns>
        public HelpConfigurator SetOptionAbr(string abr)
        {
            _abr = abr;

            return this;
        }

        /// <summary>
        /// Sets text for help option printed in help.
        /// </summary>
        /// <param name="description">Text for help option printed in help.</param>
        /// <returns>Self instance</returns>
        public HelpConfigurator SetOptionDescription(string description)
        {
            _description = description;

            return this;
        }

        /// <summary>
        /// Sets help printer to generate help text.
        /// </summary>
        /// <param name="printer">Help printer</param>
        /// <returns>Self instance</returns>
        public HelpConfigurator SetPrinter(IHelpPrinter printer)
        {
            _printer = printer;

            return this;
        }

        internal HelpConfig GetHelpConfig()
        {
            return new HelpConfig(_flag, _abr, _description, _printer);
        }
    }
}