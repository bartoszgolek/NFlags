using System.Text;

namespace NFlags.Commands
{
    internal class PrintHelpCommandExecutionContext : CommandExecutionContext
    {
        private const int ErrorExitCode = 255;

        public PrintHelpCommandExecutionContext(string additionalPrefixMessage, CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                var exitCode = 0;
                var stringBuilder = new StringBuilder();

                if (additionalPrefixMessage != "")
                {
                    exitCode = ErrorExitCode;
                    stringBuilder.AppendLine(additionalPrefixMessage);
                    stringBuilder.AppendLine();
                }
                
                stringBuilder.Append(commandConfig.NFlagsConfig.HelpPrinter.PrintHelp(commandConfig));

                output.Write(stringBuilder.ToString());

                return exitCode;
            }, null)
        {
        }
    }
}