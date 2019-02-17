using NFlags.Commands;

namespace NFlags.Kestrel
{
    internal static class Options
    {
        public const string MaxConcurrentConnections = "max-concurrent-connections";
        public const string Port = "port";
        public const string Greeting = "greeting";


        public static CommandConfigurator RegisterOptions(this CommandConfigurator c)
        {
            return c.RegisterOption<int>(b => b
                    .Name(Port)
                    .Abr("p")
                    .Description("Listening port")
                    .EnvironmentVariable("PORT")
                    .ConfigPath("Server:Port")
                    .DefaultValue(5000)
                )
                .RegisterOption<int>(b => b
                    .Name(MaxConcurrentConnections)
                    .Description("Maximum number of open connections")
                    .ConfigPath("Server:MaxConcurrentConnections")
                    .DefaultValue(5000)
                )
                .RegisterOption<string>(b => b
                    .Name(Greeting)
                    .Description("Greeting message.")
                    .ConfigPath("Greeting")
                    .DefaultValue("Hello World!")
                );
        }
    }
}