namespace NFlags
{
    /// <summary>
    /// Configuration of NFlags help generation
    /// </summary>
    public class HelpConfig
    {
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

        /// <summary>
        /// Help printer to generate help text
        /// </summary>
        public IHelpPrinter Printer { get; }

        /// <summary>
        /// Creates new instance of HelpConfig
        /// </summary>
        /// <param name="flag">Help flag for print help option</param>
        /// <param name="abr">Help flag abbreviation for print help option</param>
        /// <param name="description">Text for help option printed in help</param>
        /// <param name="helpPrinter">Help printer to generate help text</param>
        public HelpConfig(string flag, string abr, string description, IHelpPrinter helpPrinter)
        {
            Description = description;
            Printer = helpPrinter;
            Flag = flag;
            Abr = abr;
        }
    }
}