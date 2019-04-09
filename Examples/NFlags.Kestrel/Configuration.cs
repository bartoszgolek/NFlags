using System.IO;
using Microsoft.Extensions.Configuration;

namespace NFlags.Kestrel
{
    public class Configuration : IGenericConfig
    {
        private readonly IConfigurationRoot _configuration;

        public Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        public bool Has(string path)
        {
            return _configuration.GetSection(path).Exists();
        }

        public T Get<T>(string path)
        {
            return _configuration.GetSection(path).Get<T>();
        }
    }
}