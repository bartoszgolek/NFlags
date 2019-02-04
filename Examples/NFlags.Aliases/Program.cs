namespace NFlags.Aliases
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            NFlags
                .Configure(c => c
                    .SetDialect(Dialect.Gnu)
                )
                .Root(c => c
                    .RegisterFlag(b => b
                        .Name("flag")
                        .Abr("f")
                        .DefaultValue(true)
                    )
                    .RegisterFlag(b => b
                        .Name("flag-alias")
                        .Abr("fa")
                        .DefaultValue(true)
                        .Target("flag")
                    )
                    .RegisterOption<string>(b => b
                        .Name("option")
                        .Abr("o")
                        .DefaultValue("xyz")
                    )
                    .RegisterOption<string>(b => b
                        .Name("option-alias")
                        .Abr("oa")
                        .DefaultValue("asd")
                        .Target("option")
                    )
                    .SetExecute((arguments, output) =>
                    {
                        output.WriteLine("flag: " + arguments.GetFlag("flag"));
                        output.WriteLine("option: " + arguments.GetOption<string>("option"));
                    })
                )
                .Run(args);
        }
    }
}