namespace NFlags.Converters
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return NFlags
                .Configure(c => c
                    .RegisterConverter(new UserConverter())
                    .SetDescription("Call 'NFlags.Converters \"user1;user1name;user1password\" \"user2;user2name;user2password\"' to see converters in use.")
                )
                .Root(c => c
                    .RegisterParameterSeries<User>("User", "User to print")
                    .SetExecute((commandArgs, output) =>
                    {
                        var no = 1;
                        foreach (var parameter in commandArgs.ParameterSeries)
                        {
                            output.WriteLine($"No. {no++}");
                            var u = (User) parameter;
                            output.WriteLine($"UserName. {u.UserName}");
                            output.WriteLine($"Name. {u.Name}");
                            output.WriteLine($"Password. {u.Password}");
                            output.WriteLine();
                        }
                    })
                )
                .Run(args);
        }
    }
}