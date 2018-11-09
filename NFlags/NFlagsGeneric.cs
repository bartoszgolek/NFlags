using System;
using NFlags.Commands;
using NFlags.GenericCommandExtension;

namespace NFlags
{
    public static class NFlagsGeneric
    {
        /// <summary>
        /// Configure and get root command
        /// </summary>
        /// <param name="nFlags">NFlags instance</param>
        /// <param name="configureRootCommand">Action to configure command using CommandConfigurator</param>
        public static Bootstrap Root<TArguments>(this NFlags nFlags, Action<CommandConfigurator<TArguments>> configureRootCommand)
        {
            var commandConfigurator = new CommandConfigurator<TArguments>(
                new CommandConfigurator("", "", nFlags.NFlagsConfig)
            );
            configureRootCommand(commandConfigurator);

            return new Bootstrap(nFlags.NFlagsConfig, commandConfigurator.CreateCommand());
        }      
    }
}