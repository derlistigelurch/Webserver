namespace Webserver
{
    /// <summary>
    /// Saves Configurationdata (e.g. StaticFileDirectory).
    /// </summary>
    public class Configuration
    {
        private static Configuration _currentConfiguration;

        /// <summary>
        /// Creates a new static Configuration object.
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