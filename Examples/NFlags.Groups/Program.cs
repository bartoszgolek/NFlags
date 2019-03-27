namespace NFlags.Groups
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
                    .RegisterParameter<string>(b => b.Name("param1"))
                    .RegisterFlag(b => b.Name("flag1"))
                    .RegisterFlag(b => b.Name("group1-flag1").Group("group1"))
                    .RegisterFlag(b => b.Name("group2-flag1").Group("group2"))
                    .RegisterFlag(b => b.Name("group1-flag2").Group("group1"))
                    .RegisterFlag(b => b.Name("flag2"))
                    .RegisterOption<string>(b => b.Name("option1"))
                    .RegisterOption<string>(b => b.Name("group1-option1").Group("group1"))
                    .RegisterOption<string>(b => b.Name("group2-option1").Group("group2"))
                    .RegisterOption<string>(b => b.Name("group1-option2").Group("group1"))
                    .RegisterOption<string>(b => b.Name("option2"))
                    .PrintHelpOnExecute()
                )
                .Run(args);
        }
    }
}