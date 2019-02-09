using Microsoft.Extensions.Configuration;
using System.IO;

namespace NFlags.Configuration
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath (Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                ;
            
            
            var configuration = builder.Build();

            NFlags.Configure(c => c
                    .SetDialect(Dialect.Gnu)
                    .SetConfiguration(new ConfigurationExtensionProvider(configuration))
                )
                .Root(c => c
                    .RegisterOption<string>(b => b
                        .Name("asd")
                        .EnvironmentVariable("asd")
                        .ConfigPath("Consul:Address")
                    )
                )
                .Run(args);
        }
    }

    internal class ConfigurationExtensionProvider : IConfig
    {
        private readonly IConfiguration _configuration;

        public ConfigurationExtensionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get(string path)
        {
            return _configuration.GetValue<string>(path);
        }
    }
}