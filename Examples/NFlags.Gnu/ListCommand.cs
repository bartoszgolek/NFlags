using System;
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
                .RegisterParam(Text, "Text to show", "Default text to show")
                .SetExecute(Execute);
        }

        private static void Execute(CommandArgs commandArgs, Action<string> outout)
        {
            outout("List: " + commandArgs.Parameters[Text]);
        }
    }
}