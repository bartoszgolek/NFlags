using System;
using NFlags.Commands;

namespace NFlags.Gnu
{
    public static class RootCommand
    {
        private const string Verbose = "verbose";
        private const string Clear = "clear";
        private const string Option1 = "option1";
        private const string Option2 = "option2";
        private const string Param1 = "param1";

        public static void Configure(CommandConfigurator configurator)
        {
            configurator
                .RegisterFlag(Verbose, "v", "Verbose description", false)
                .RegisterFlag(Clear, "Clear description", false)
                .RegisterOption(Option1, "o1", "Option 1 description", "default")
                .RegisterOption(Option2, "Option 2 description", "default2")
                .RegisterParam(Param1, "Parameter 1 description", ".")
                .SetExecute(Execute)
                .RegisterSubcommand(ShowCommand.Name, ShowCommand.Description, ShowCommand.Configure)
                .RegisterSubcommand(ListCommand.Name, "List somethig", ListCommand.Configure);
        }

        private static void Execute(CommandArgs commandArgs, Action<string> output)
        {
            output("Verbose: " + commandArgs.Flags[Verbose]);
            output("Clear: " + commandArgs.Flags[Clear]);
            output("Option1: " + commandArgs.Options[Option1]);
            output("Option2: " + commandArgs.Options[Option2]);
            output("Param1: " + commandArgs.Parameters[Param1]);
        }
    }
}