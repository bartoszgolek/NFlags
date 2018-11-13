using NFlags.Arguments;
using NFlags.TypeConverters;

namespace NFlags.Commands
{
    /// <summary>
    /// Represents registered application command.
    /// </summary>
    public class Command
    {
        private const string HelpFlag = "help";
        private const string HelpFlagAbr = "h";
        private const string HelpDescription = "Prints this help";

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
            _commandConfig.Flags.Add(
                new Flag
                {
                    Name = HelpFlag,
                    Abr = HelpFlagAbr,
                    Description = HelpDescription,
                    DefaultValue = false
                }
            );

            try
            {
                var commandExecutionContext = new CommandExecutionContextProvider(
                    _commandConfig,
                    args
                ).GetFromArgs();

                if (_commandConfig.PrintHelpOnExecute)
                    return PrepareHelpCommandExecutionContext();

                return commandExecutionContext.Args != null && commandExecutionContext.Args.GetFlag(HelpFlag)
                    ? PrepareHelpCommandExecutionContext()
                    : commandExecutionContext;
            }
            catch (ArgumentValueException e)
            {
                if (!_commandConfig.NFlagsConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(e.Message);
            }
            catch (TooManyParametersException e)
            {
                if (!_commandConfig.NFlagsConfig.IsExceptionHandlingEnabled)
                    throw;

                return PrepareHelpCommandExecutionContext(e.Message);
            }
        }

        private PrintHelpCommandExecutionContext PrepareHelpCommandExecutionContext(string additionalPrefixMessage = "")
        {
            return new PrintHelpCommandExecutionContext(additionalPrefixMessage, _commandConfig);
        }
    }
}