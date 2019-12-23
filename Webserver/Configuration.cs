using Webserver.Plugins;

namespace Webserver
{
    public class Configuration
    {
        private static Configuration _currentConfiguration;

        private Configuration()
        {
        }

        public static Configuration CurrentConfiguration =>
            _currentConfiguration ?? (_currentConfiguration = new Configuration());

        public string StaticFileDirectory { get; set; }
    }
}