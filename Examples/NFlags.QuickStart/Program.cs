namespace NFlags.QuickStart
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Cli.Configure(configure => configure
                .SetDialect(Dialect.Gnu)
                .SetName("QuickStart")
                .SetDescription("This is NFlags")
                .ConfigureVersion(vc => vc.Enable())
            ).
            Root(rc => rc.
                RegisterFlag("flag1", "f", "Flag description", false).
                RegisterOption("option", "o", "Option description", "optionDefaultValue").
                RegisterParameter("param", "Param description", "ParamDefaultValue").
                RegisterCommand("command", "Sub command Description", sc => sc.
                        SetExecute((commandArgs, output) =>
                        {
                            output.WriteLine("This is sub command: {0}", commandArgs.GetParameter<string>("Parameter"));
                        }).
                        RegisterParameter("Parameter", "Sub parameter description", "SubParameterValue")
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