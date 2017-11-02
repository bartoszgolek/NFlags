using System;

namespace NFlags
{
    public class ParamReader
    {
        private readonly NFlagsConfig _nFlagsConfig;

        public ParamReader(NFlagsConfig nFlagsConfig)
        {
            _nFlagsConfig = nFlagsConfig;
        }

        /// <summary>
        /// Configure and get root command
        /// </summary>
        /// <param name="configureCommand">Action to configure command using CommandConfigurator</param>
        public Command Root(Action<CommandConfigurator> configureCommand)
        {
            var commandConfigurator = new CommandConfigurator(_nFlagsConfig);
            configureCommand(commandConfigurator);
            return commandConfigurator.CreateCommand();
        }

        /// <summary>
        /// Get root command
        /// </summary>
        public Command Root()
        {
            return Root(c => {});
        }
    }
}