using NFlags.Commands;

namespace NFlags.Gnu
{
    public static class ListCommand
    {
        public const string Name = "list";
        private const string Text = "text";

        public static void Configure(CommandConfigurator configurator)
        {
            configurator
                .RegisterParameter(Text, "Text to show", "Default text to show")
                .SetExecute(Execute);
        }

        private static int Execute(CommandArgs commandArgs, IOutput output)
        {
            output.Write("List: {0}", commandArgs.GetParameter<string>(Text));
            return 0;
        }
    }
}