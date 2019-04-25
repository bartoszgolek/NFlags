using System;
using NFlags.Commands;

namespace NFlags
{
    /// <summary>
    /// Provides API for NFlags usage.
    /// </summary>
    [Obsolete("NFlags class is obsolete. Use Cli instead.")]
    public class NFlags
    {
        /// <summary>
        /// Configure NFLags param reader.
        /// </summary>
        /// <param name="configurator">Param reader configurator.</param>
        /// <returns>Configured param reader.</returns>
        public static Cli Configure(Action<CliConfigurator> configurator)
        {
            return Cli.Configure(configurator);
        }
    }
}