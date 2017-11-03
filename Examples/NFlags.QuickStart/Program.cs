namespace NFlags.QuickStart
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags.Configure(configure => configure
                .SetDialect(Dialect.Gnu)
                .SetName("QuickStart")
                .SetDescription("This is NFlags")
            ).
            Root(rc => rc.
                RegisterFlag("flag1", "f", "Flag description", false).
                RegisterOption("option", "o", "Option description", "optionDefaultValue").
                RegisterParam("param", "Param description", "ParamDefaultValue").
                RegisterSubcommand("subcommand", "Subcommand Description", sc => sc.
                        SetExecute((commandArgs, output) => output("This is subcommand: " + commandArgs.Parameters["SubParameter"])).
                        RegisterParam("SubParameter", "SubParameter description", "SubParameterValue")
                ).
                SetExecute((commandArgs, output) => output("This is root command: " + commandArgs.Parameters["param"]))
            ).
            Run(args);
        }
    }
}