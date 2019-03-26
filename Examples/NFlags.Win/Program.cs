namespace NFlags.Win
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags.Configure(ConfigureNFlags)
                .Root(RootCommand.Configure)
                .Run(args);
        }

        private static void ConfigureNFlags(NFlagsConfigurator configurator)
        {
            configurator
                .SetDescription("Application description")
                .SetDialect(Dialect.Win)
                .SetOutput(Output.Console);
        }
    }
}