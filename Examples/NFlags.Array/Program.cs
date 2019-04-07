namespace NFlags.Array
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags.Configure(c => c
                    .SetDescription("Application description")
                    .SetDialect(Dialect.Gnu)
                    .SetOutput(Output.Console))
                .Root(c => c
                    .RegisterOption<string[]>(b => b.Name("array-option").DefaultValue(new string[0]))
                    .SetExecute((commandArgs, output) =>
                    {
                        foreach (var option in commandArgs.GetOption<string[]>("carray-option"))
                            output.WriteLine(option);
                    })
                )
                .Run(args);
        }
    }
}