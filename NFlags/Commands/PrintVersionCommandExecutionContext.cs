using System.Reflection;

namespace NFlags.Commands
{
    internal class PrintVersionCommandExecutionContext : CommandExecutionContext
    {
        public PrintVersionCommandExecutionContext(CliConfig cliConfig, CommandConfig commandConfig)
            : base((commandArgs, output) =>
            {
                output.WriteLine(cliConfig.Name + " Version: " + Assembly.GetEntryAssembly()?.GetName().Version);

                return 0;
            }, null)
        {
        }
    }
}