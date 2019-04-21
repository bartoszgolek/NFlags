namespace NFlags.Converters
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Cli
                .Configure(c => c
                    .RegisterConverter(new UserConverter())
                    .SetDescription("Call 'NFlags.Converters \"user1;user1name;user1password\" \"user2;user2name;user2password\"' to see converters in use.")
                )
                .Root(c => c
                    .RegisterParameterSeries<User>("User", "User to print")
                    .SetExecute((commandArgs, output) =>
                    {
                        var no = 1;
                        foreach (var user in commandArgs.GetParameterSeries<User>())
                        {
                            output.WriteLine($"No. {no++}");
                            output.WriteLine($"UserName. {user.UserName}");
                            output.WriteLine($"Name. {user.Name}");
                            output.WriteLine($"Password. {user.Password}");
                            output.WriteLine();
                        }
                    })
                )
                .Run(args);
        }
    }
}