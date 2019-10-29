using System.Reflection;

namespace NFlags.Commands
{
    internal class PrintVersionCommandExecutionContext : CommandExecutionContext
    {
        public PrintVersionCommandExecutionContext(CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                output.WriteLine(commandConfig.CliConfig.Name + " Version: " + Assembly.GetEntryAssembly()?.GetName().Version);

                return 0;
            }, null)
        {
        }
    }
}