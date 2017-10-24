using System;
using Microsoft.Extensions.Configuration;

namespace Example.Common
{
    public class Configuration
    {
        public Configuration()
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string AccessKey = "";
        public static string SecretKey = "";
    }
}
