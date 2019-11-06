namespace NFlags.Commands
{
    internal class CommandArgsParseContext
    {
        public CommandArgsParseContext(CommandConfig commandConfig, string[] args)
        {
            CommandConfig = commandConfig;
            Args = args;
        }

        public CommandConfig CommandConfig { get; }

        public string[] Args { get; }
    }
}