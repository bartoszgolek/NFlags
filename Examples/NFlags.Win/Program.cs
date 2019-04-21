namespace NFlags.Win
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Cli.Configure(ConfigureNFlags)
                .Root(RootCommand.Configure)
                .Run(args);
        }

        private static void ConfigureNFlags(CliConfigurator configurator)
        {
            configurator
                .SetDescription("Application description")
                .SetDialect(Dialect.Win)
                .SetOutput(Output.Console);
        }
    }
}