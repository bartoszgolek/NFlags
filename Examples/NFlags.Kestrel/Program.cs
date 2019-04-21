using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NFlags.Commands;

namespace NFlags.Kestrel
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Cli.Configure(c => c
                    .SetName("NFlags.Kestrel")
                    .SetDescription("NFlags Kestrel Example")
                    .SetDialect(Dialect.Gnu)
                    .SetEnvironment(Environment.Prefixed("NFLAGS_KESTREL_"))
                    .SetConfiguration(new Configuration())
                )
                .Root(c => c
                    .RegisterOptions()
                    .SetExecute((commandArgs, output) => RunKestrel(commandArgs))
                )
                .Run(args);
        }

        private static void RunKestrel(CommandArgs commandArgs)
        {
            var port = commandArgs.GetOption<int>(Options.Port);
            var maxConcurrentConnections = commandArgs.GetOption<int>(Options.MaxConcurrentConnections);
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices(collection => collection.AddSingleton(commandArgs))
                .UseKestrel()
                .ConfigureKestrel((context, options) =>
                    {
                        options.Limits.MaxConcurrentConnections = maxConcurrentConnections;
                    })
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:" + port)
                .Build();

            host.Run();
        }
    }
}