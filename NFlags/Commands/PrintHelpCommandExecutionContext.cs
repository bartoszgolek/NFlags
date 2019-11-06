namespace NFlags.Commands
{
    internal class PrintHelpCommandExecutionContext : CommandExecutionContext
    {
        private const int ErrorExitCode = 255;

        public PrintHelpCommandExecutionContext(string additionalPrefixMessage, CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                output.WriteLine(additionalPrefixMessage);
                output.WriteLine();
                output.Write(commandConfig.CliConfig.HelpConfig.Printer.PrintHelp(commandConfig));

                return ErrorExitCode;
            }, null)
        {
        }

        public PrintHelpCommandExecutionContext(CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                output.Write(commandConfig.CliConfig.HelpConfig.Printer.PrintHelp(commandConfig));

                return 0;
            }, null)
        {
        }
    }
}