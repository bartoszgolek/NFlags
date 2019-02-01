using NFlags.Commands;

namespace NFlags.Builders
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
                .RegisterFlag(b => b.Name(Verbose).Abr("v").Description("Verbose description").DefaultValue(false))
                .RegisterFlag(b => b.Name(Clear).Description("Clear description").DefaultValue(false))
                .RegisterOption<string>(b => b.Name(Option1).Abr("o1").Description("Option 1 description").DefaultValue("default"))
                .RegisterOption<string>(b => b.Name(Option2).Description("Option 2 description").DefaultValue("default2"))
                .RegisterParameter<string>(b => b.Name(Param1).Description("Parameter 1 description").DefaultValue("."))
                .SetExecute(Execute);
        }

        private static int Execute(CommandArgs commandArgs, IOutput output)
        {
            output.WriteLine("Verbose: {0}", commandArgs.GetFlag(Verbose));
            output.WriteLine("Clear: {0}", commandArgs.GetFlag(Clear));
            output.WriteLine("Option1: {0}", commandArgs.GetOption<string>(Option1));
            output.WriteLine("Option2: {0}", commandArgs.GetOption<string>(Option2));
            output.WriteLine("Param1: {0}", commandArgs.GetParameter<string>(Param1));
            return 0;
        }
    }
}