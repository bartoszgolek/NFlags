using NFlags.Utils;

namespace NFlags
{
    /// <summary>
    /// Prints help based on CommandConfig
    /// </summary>
    public static class HelpPrinter
    {
        /// <summary>
        /// Default help printer implementation.
        /// </summary>
        public static readonly IHelpPrinter Default = new DefaultHelpPrinter();

        /// <summary>
        /// Default help printer implementation.
        /// </summary>
        public static IHelpPrinter DefaultWithOptions(DefaultHelpPrinterOptions options)
        {
            return new DefaultHelpPrinter(options);
        }
    }
}