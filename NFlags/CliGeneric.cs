using System;
using NFlags.Commands;
using NFlags.GenericCommandExtension;

namespace NFlags
{
    /// <summary>
    /// NFlags extension to register root command with custom args type.
    /// </summary>
    public static class CliGeneric
    {
        /// <summary>
        /// Configure and get root command
        /// </summary>
        /// <param name="cli">NFlags Cli instance</param>
        /// <param name="configureRootCommand">Action to configure command using CommandConfigurator</param>
        public static Bootstrap Root<TArguments>(this Cli cli, Action<CommandConfigurator<TArguments>> configureRootCommand)
        {
            var commandConfigurator = new CommandConfigurator<TArguments>(
                new CommandConfigurator("", "", cli.CliConfig)
            );
            configureRootCommand(commandConfigurator);

            return new Bootstrap(cli.CliConfig, commandConfigurator.GetCommandConfig());
        }
    }
}