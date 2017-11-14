using NFlags.Utils;

namespace NFlags
{
    /// <summary>
    /// Represents ability to write output.
    /// </summary>
    public static class Output
    {
        /// <summary>
        /// Output implementation to print to standard output stream.
        /// </summary>
        public static readonly IOutput Console = new ConsoleOutput();
    }
}