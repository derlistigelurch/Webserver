namespace Webserver
{
    public class Configuration
    {
        private static Configuration _currentConfiguration;

        /// <summary>
        /// Create a new static Configuration object, to store data
        /// </summary>
        public static Configuration CurrentConfiguration
        {
            get
            {
                var configuration = _currentConfiguration;
                if (configuration != null)
                {
                    return configuration;
                }

                return (_currentConfiguration = new Configuration());
            }
        }

        /// <summary>
        /// Returns or sets the static files directory.
        /// </summary>
        public string StaticFileDirectory { get; set; }
    }
}