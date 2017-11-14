using NFlags.Commands;

namespace NFlags.Gnu
{
    public static class ShowCommand
    {
        public const string Name = "show";
        public const string Description = "Show somethig";

        private const string Text = "text";

        public static void Configure(CommandConfigurator configurator)
        {
            configurator
                .RegisterParam(Text, "Text to show", "Default text to show")
                .SetExecute(Execute)
                .RegisterSubcommand("subshow", "Show somethig", SubShowCommand.Configure);
        }

        private static void Execute(CommandArgs commandArgs, IOutput output)
        {
            output.WriteLine("Show: {0}", commandArgs.Parameters[Text]);
        }
    }
}