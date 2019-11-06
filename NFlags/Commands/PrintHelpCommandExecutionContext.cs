namespace NFlags.Commands
{
    internal class PrintHelpCommandExecutionContext : CommandExecutionContext
    {
        private const int ErrorExitCode = 255;

        public PrintHelpCommandExecutionContext(string additionalPrefixMessage, CliConfig cliConfig, CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                output.WriteLine(additionalPrefixMessage);
                output.WriteLine();
                output.Write(cliConfig.HelpConfig.Printer.PrintHelp(cliConfig, commandConfig));

                return ErrorExitCode;
            }, null)
        {
        }

        public PrintHelpCommandExecutionContext(CliConfig cliConfig, CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                output.Write(cliConfig.HelpConfig.Printer.PrintHelp(cliConfig, commandConfig));

                return 0;
            }, null)
        {
        }
    }
}