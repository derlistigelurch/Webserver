using System;
using System.IO;

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
        public static Configuration CurrentConfiguration =>
            _currentConfiguration ?? (_currentConfiguration = new Configuration());
        // if(_currentConfiguration) == null
        
        /// <summary>
        /// Returns or sets the static files directory.
        /// </summary>
        public string StaticFileDirectory { get; set; }

        /// <summary>
        /// Returns the Plugin Directory
        /// </summary>
        public string PluginDirectory => Path.Combine(System.Environment.CurrentDirectory, "plugins");

        /// <summary>
        /// Returns the osm-file Directory
        /// </summary>
        public string OsmDirectory =>
            Path.Combine(System.Environment.CurrentDirectory,
                this.StaticFileDirectory,
                "osm_data");
    }
}