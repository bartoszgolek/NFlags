namespace NFlags.Generics
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root<RootCommandArguments>(c => c
                    .PrintHelpOnExecute()
                    .RegisterCommand<RootCommandArguments>("command1", "this is command 1", configurator => configurator
                        .PrintHelpOnExecute()
                    )
                )
                .Run(args);
        }
    }
}