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

        private static void Execute(CommandArgs commandArgs, IOutput output)
        {
            output.WriteLine("Verbose: {0}", commandArgs.Flags[Verbose]);
            output.WriteLine("Clear: {0}", commandArgs.Flags[Clear]);
            output.WriteLine("Option1: {0}", commandArgs.Options[Option1]);
            output.WriteLine("Option2: {0}", commandArgs.Options[Option2]);
            output.WriteLine("Param1: {0}", commandArgs.Parameters[Param1]);
        }
    }
}