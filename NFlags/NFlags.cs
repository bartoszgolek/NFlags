using System;
using NFlags.Commands;

namespace NFlags
{
    /// <summary>
    /// Provides API for NFlags usage.
    /// </summary>
    public class NFlags
    {
        /// <summary>
        /// Configure NFLags param reader.
        /// </summary>
        /// <param name="configurator">Param reader configurator.</param>
        /// <returns>Configured param reader.</returns>
        public static NFlags Configure(Action<NFlagsConfigurator> configurator)
        {
            var paramReaderConfigurator = new NFlagsConfigurator();
            configurator(paramReaderConfigurator);
            return paramReaderConfigurator.CreateNFlags();
        }

        private readonly NFlagsConfig _nFlagsConfig;

        internal NFlags(NFlagsConfig nFlagsConfig)
        {
            _nFlagsConfig = nFlagsConfig;
        }

        /// <summary>
        /// Configure and get root command
        /// </summary>
        /// <param name="configureRootCommand">Action to configure command using CommandConfigurator</param>
        public Bootstrap Root(Action<CommandConfigurator> configureRootCommand)
        {
            var commandConfigurator = new CommandConfigurator("", "", _nFlagsConfig);
            configureRootCommand(commandConfigurator);

            return new Bootstrap(_nFlagsConfig, commandConfigurator.CreateCommand());
        }
    }
}