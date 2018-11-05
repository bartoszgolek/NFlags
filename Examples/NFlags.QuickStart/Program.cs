namespace NFlags.QuickStart
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return NFlags.Configure(configure => configure
                .SetDialect(Dialect.Gnu)
                .SetName("QuickStart")
                .SetDescription("This is NFlags")
            ).
            Root(rc => rc.
                RegisterFlag("flag1", "f", "Flag description", false).
                RegisterOption("option", "o", "Option description", "optionDefaultValue").
                RegisterParam("param", "Param description", "ParamDefaultValue").
                RegisterSubcommand("subcommand", "Subcommand Description", sc => sc.
                        SetExecute((commandArgs, output) =>
                        {
                            output.WriteLine("This is subcommand: {0}", commandArgs.GetParameter<string>("SubParameter"));
                        }).
                        RegisterParam("SubParameter", "SubParameter description", "SubParameterValue")
                ).
                RegisterParameterSeries<string>("paramSeries", "paramSeriesDescription").
                SetExecute((commandArgs, output) =>
                    {
                        output.WriteLine("This is root command: {0}", commandArgs.GetParameter<string>("param"));
                        return 0;
                    })
            ).
            Run(args);
        }
    }
}