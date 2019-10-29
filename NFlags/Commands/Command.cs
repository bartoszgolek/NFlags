namespace NFlags.Commands
{
    /// <summary>
    /// Represents registered application command.
    /// </summary>
    public class Command
    {
        private readonly CommandConfig _commandConfig;

        /// <summary>
        /// Creates new command instance.
        /// </summary>
        /// <param name="commandConfig">Command configuration.</param>
        public Command(CommandConfig commandConfig)
        {
            _commandConfig = commandConfig;
        }

        /// <summary>
        /// Read and parse arguments.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        public CommandExecutionContext Read(string[] args)
        {
            return new CommandExecutionContextProvider(
                _commandConfig,
                args
            ).GetFromArgs();
        }
    }
}