using Microsoft.Extensions.Configuration;

namespace Example.Common
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot ConfigurationRoot;

        static Configuration()
        {
            ConfigurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string AccessKey => ConfigurationRoot["AWS:AccessKey"];
        public static string SecretKey => ConfigurationRoot["AWS:SecretKey"];
    }
}
