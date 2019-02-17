using System.IO;
using Microsoft.Extensions.Configuration;

namespace NFlags.Kestrel
{
    public class Configuration : IConfig
    {
        private readonly IConfigurationRoot _configuration;

        public Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        public string Get(string path)
        {
            return _configuration.GetValue<string>(path);
        }
    }
}