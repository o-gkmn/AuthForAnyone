using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration
{
    public class ConfigurationFactory
    {
        private static ConfigurationFactory instance;
        public IConfiguration Configuration { get; }

        private ConfigurationFactory()
        {
            Configuration = CreateInstanceOfIConfiguration();
        }

        public static ConfigurationFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new ConfigurationFactory();
            }

            return instance;
        }

        private IConfiguration CreateInstanceOfIConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(ApplicationDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private string ApplicationDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);
            return appRoot;
        }
    }
}
