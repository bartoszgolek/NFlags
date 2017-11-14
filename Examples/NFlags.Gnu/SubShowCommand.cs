using NFlags.Commands;

namespace NFlags.Gnu
{
    public static class SubShowCommand
    {
        public static void Configure(CommandConfigurator configurator)
        {
            configurator
                .RegisterParam("text2", "Text to show", "Default new text to show")
                .SetExecute(Execute);
        }

        private static void Execute(CommandArgs commandArgs, IOutput output)
        {
            output.WriteLine(commandArgs.Parameters["text"]);
        }
    }
}