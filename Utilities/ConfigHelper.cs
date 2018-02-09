using System.IO;
using Microsoft.Extensions.Configuration;

namespace Utilities
{
    public class ConfigHelper
    {
        public static string Get(string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json");
            var config = builder.Build();
            return config.GetSection(key).Value;
        }
    }
}
