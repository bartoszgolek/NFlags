namespace NFlags.Win
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return NFlags.Configure(configurator => configurator
                    .SetName("Custom Name")
                    .SetDescription("My command app")
                    .SetOutput(Output.Console)
                )
                .Root(configurator => configurator
                    .RegisterFlag("verbose", "v", "Verbose description", false)
                    .RegisterFlag("clear", "Clear description", false)
                    .RegisterOption("option1", "o1", "Option 1 description", "default")
                    .RegisterOption("option2", "Option 2 description", "default1")
                    .RegisterParam("param1", "Parameter 1 description", ".")
                    .SetExecute((commandArgs, output) =>
                    {
                        output.WriteLine("Verbose: {0}", commandArgs.Flags["verbose"].ToString());
                        output.WriteLine("Clear: {0}", commandArgs.Flags["clear"].ToString());
                        output.WriteLine("Option1: {0}", commandArgs.Options["option1"]);
                        output.WriteLine("Option2: {0}", commandArgs.Options["option2"]);
                        output.WriteLine("Param1: {0}", commandArgs.Parameters["param1"]);
                        
                        return 0;
                    })
                )
                .Run(args);
        }
    }
}