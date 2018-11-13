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
                .RegisterParameter(Text, "Text to show", "Default text to show")
                .SetExecute(Execute)
                .RegisterCommand("sub_show", "Show something", SubShowCommand.Configure);
        }

        private static int Execute(CommandArgs commandArgs, IOutput output)
        {
            output.WriteLine("Show: {0}", commandArgs.GetParameter<string>(Text));
            return 0;
        }
    }
}