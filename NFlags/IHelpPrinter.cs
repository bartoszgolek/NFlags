using NFlags.Commands;

namespace NFlags
{
    /// <summary>
    /// Prints help based on CommandConfig
    /// </summary>
    public interface IHelpPrinter
    {
        /// <summary>
        /// Create help text based on CommandConfig 
        /// </summary>
        /// <param name="commandConfig">Command configuration</param>
        /// <returns>Help text</returns>
        string PrintHelp(CommandConfig commandConfig);
    }
}