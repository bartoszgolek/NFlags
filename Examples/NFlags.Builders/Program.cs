namespace NFlags.Builders
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
                .SetDialect(Dialect.Gnu)
                .SetOutput(Output.Console);
        }
    }
}