namespace NFlags.GenericAliases
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root<CommandArguments>(c => c
                    .SetExecute((arguments, output) =>
                    {
                        output.WriteLine("flag: " +  arguments.Flag);
                        output.WriteLine("option: " + arguments.Option);
                    })
                )
                .Run(args);
        }
    }
}