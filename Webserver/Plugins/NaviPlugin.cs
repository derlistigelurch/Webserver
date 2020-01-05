using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using OsmSharp.Streams;

namespace Webserver.Plugins
{
    /// <summary>
    /// A Plugin which shows streets in a given area
    /// </summary>
    public class NaviPlugin : IPlugin
    {
        // street : city
        // HashSet: no duplicates
        private static readonly Dictionary<string, HashSet<string>> StreetCityDictionary =
            new Dictionary<string, HashSet<string>>();

        private static readonly object RefreshLock = new object();
        private static bool _isRefreshing = false;

        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            // POST
            if (string.IsNullOrEmpty(req.ContentString) == false &&
                (req.ContentString.Contains("street") || req.ContentString.Contains("refresh")))
            {
                return 1.0f;
            }

            return 0.0f;
        }

        /// <summary>
        /// Called by the server when the plugin should handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            var response = new Response() {StatusCode = 200};
            
            lock (RefreshLock)
            {
                if (_isRefreshing)
                {
                    response.SetContent(this.CreateNaviHtml("navi.html",
                        "<script>alert('Karte wird gerade neu geladen');</script>"));
                    return response;
                }
            }
            
            if (req.ContentString.Contains("street"))
            {
                //street=Triester+Stra%C3%9Fe&navigation=
                response.SetContent(
                    LoadCities(ParseCharacters(req.ContentString.Split('&').First()
                        .Substring(req.ContentString.IndexOf('=') + 1))));
            }
            else
            {
                ReloadMap();
            }

            return response;
        }

        /// <summary>
        /// Reload the Street-City-Dictionary
        /// </summary>
        private void ReloadMap()
        {
            lock (RefreshLock)
            {
                if (_isRefreshing)
                {
                    return;
                }

                _isRefreshing = true;
            }

            using (var fileStream =
                new FileInfo(Path.Combine(Configuration.CurrentConfiguration.OsmDirectory, "austria.osm.pbf"))
                    .OpenRead())
            {
                var i = 0;
                var sw = new Stopwatch();
                Console.WriteLine("Starting...");
                sw.Start();

                var source = new PBFOsmStreamSource(fileStream);
                foreach (var element in source)
                {
                    if (element.Tags.ContainsKey("addr:city") && element.Tags.ContainsKey("addr:street"))
                    {
                        i++;
                        if (StreetCityDictionary.ContainsKey(element.Tags["addr:street"]))
                        {
                            StreetCityDictionary[element.Tags["addr:street"]].Add(element.Tags["addr:city"]);
                        }
                        else
                        {
                            StreetCityDictionary.Add(element.Tags["addr:street"], new HashSet<string>());
                            StreetCityDictionary[element.Tags["addr:street"]].Add(element.Tags["addr:city"]);
                        }
                    }
                }

                sw.Stop();
                Console.WriteLine("Entries found: {0}", i);
                Console.WriteLine("Elapsed Time = {0}", sw.Elapsed);
            }

            lock (RefreshLock)
            {
                _isRefreshing = false;
            }
        }

        /// <summary>
        /// Search for street name and returns the city names
        /// </summary>
        /// <param name="street"></param>
        /// <returns>A string with the names of the cities</returns>
        private string LoadCities(string street)
        {
            var result = new StringBuilder();
            if (string.IsNullOrEmpty(street))
            {
                result.Append("Bitte geben Sie eine Anfrage ein");
            }
            else if (StreetCityDictionary.ContainsKey(street))
            {
                result.Append(StreetCityDictionary[street].Count.ToString()).Append(" Ort(e) gefunden<br><ul>");
                foreach (var city in StreetCityDictionary[street])
                {
                    result.Append("<li>").Append(city).Append("</li>");
                }

                result.Append("</ul>");
            }
            else
            {
                result.Append("0 Orte gefunden");
            }

            return CreateNaviHtml("navi.html", result.ToString());
        }

        /// <summary>
        /// HTML input cannot parse UTF-8 characters and " " correctly, so heres a function to do this
        /// </summary>
        /// <param name="s"></param>
        /// <returns>A new string with UTF-8 encoded Characters and whitespaces</returns>
        private string ParseCharacters(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var utfDict = new Dictionary<string, string>
            {
                {"%C3%9F", "ß"},
                {"%C3%B6", "ö"},
                {"%C3%96", "Ö"},
                {"%C3%A4", "ä"},
                {"%C3%84", "Ä"},
                {"%C3%BC", "ü"},
                {"%C3%9C", "Ü"},
                {"+", " "}
            };

            foreach (var (key, value) in utfDict)
            {
                if (s.Contains(key))
                {
                    s = s.Replace(key, value);
                }
            }

            return s;
        }

        /// <summary>
        /// Creates a new Html file for the navi plugin
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="result"></param>
        /// <returns>A new HTML file</returns>
        private string CreateNaviHtml(string filename, string result)
        {
            if (File.Exists(Path.Combine(Configuration.CurrentConfiguration.StaticFileDirectory, filename)))
            {
                return File.ReadAllText(Path.Combine(Configuration.CurrentConfiguration.StaticFileDirectory,
                    filename)).Replace("<pre id=\"result\"></pre>", result);
            }

            return null;
        }
    }
}