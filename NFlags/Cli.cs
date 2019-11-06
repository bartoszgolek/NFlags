using System;
using NFlags.Commands;

namespace NFlags
{
    /// <summary>
    /// Provides API for NFlags usage.
    /// </summary>
    public class Cli
    {
        /// <summary>
        /// Configure NFLags param reader.
        /// </summary>
        /// <param name="configurator">Param reader configurator.</param>
        /// <returns>Configured param reader.</returns>
        public static Cli Configure(Action<CliConfigurator> configurator)
        {
            var paramReaderConfigurator = new CliConfigurator();
            configurator(paramReaderConfigurator);
            return paramReaderConfigurator.CreateCli();
        }

        internal Cli(CliConfig cliConfig)
        {
            CliConfig = cliConfig;
        }

        /// <summary>
        /// Configure and get root command
        /// </summary>
        /// <param name="configureRootCommand">Action to configure command using CommandConfigurator</param>
        public Bootstrap Root(Action<CommandConfigurator> configureRootCommand)
        {
            var commandConfigurator = new CommandConfigurator("", "", CliConfig);
            configureRootCommand(commandConfigurator);

            return new Bootstrap(CliConfig, commandConfigurator.GetCommandConfig());
        }

        /// <summary>
        /// Configuration of NFlags
        /// </summary>
        public CliConfig CliConfig { get; }
    }
}