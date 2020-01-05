using System;
using System.Collections.Generic;
using System.Xml.Linq;
using BIF.SWE1.Interfaces;
using Webserver.Database;

namespace Webserver.Plugins
{
    /// <summary>
    /// A plugin which handles sensor data.
    /// </summary>
    public class TempPlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            return req?.Url != null && req.Url.Parameter.ContainsKey("GetTemperature")
                ? 1.0f
                : 0.0f;
        }

        /// <summary>
        /// Called by the server when the plugin should handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            var response = new Response() {StatusCode = 200};
            var databaseConnection = new DatabaseConnection();
            var page = 1;
            Dictionary<string, double> tempData = null;

            if (req.Url.Parameter.ContainsKey("until") && req.Url.Parameter.ContainsKey("from"))
            {
                tempData = databaseConnection.SelectTemperatureRange(
                    this.ParseToDateTime(req.Url.Parameter["from"]),
                    this.ParseToDateTime(req.Url.Parameter["until"]));
            }

            if (req.Url.Parameter.ContainsKey("date"))
            {
                tempData = databaseConnection.SelectTemperatureExact(
                    this.ParseToDateTime(req.Url.Parameter["date"]));
            }

            if (req.Url.Parameter.ContainsKey("page"))
            {
                page = Convert.ToInt32(req.Url.Parameter["page"]);
            }

            if (req.Url.Parameter.ContainsKey("type") && req.Url.Parameter["type"].Equals("rest"))
            {
                response.ContentType = "text/xml";
                response.SetContent(CreateRestNaviXml(tempData));
            }
            else
            {
                response.ContentType = "text/html";
                response.SetContent(CreateNaviHtml(tempData, page, req.Url));
            }

            return response;
        }

        /// <summary>
        /// Creates a valid XML object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A valid string which contains a valid XML object</returns>
        public string CreateRestNaviXml(Dictionary<string, double> data)
        {
            // Setup base structure:
            var xDocument = new XDocument();
            var root = new XElement("Temperature");
            xDocument.Add(root);

            // Generate the rest of the document based on runtime data:
            foreach (var (key, value) in data)
            {
                var temp = new XElement("data");
                temp.Add(new XElement("date", key));
                temp.Add(new XElement("temp", value));
                root.Add(temp);
            }

            return xDocument.ToString();
        }

        /// <summary>
        /// Create a valid HTML site.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <param name="url"></param>
        /// <returns>A valid HTML site as string.</returns>
        public string CreateNaviHtml(Dictionary<string, double> data, int page, IUrl url)
        {
            // temp.html?from=2020-01-01&until=2020-01-02&GetTemperature=
            // Setup base structure:
            var xDocument = new XDocument();
            var head = new XElement("html",
                new XAttribute("lang", "de"),
                new XElement("head",
                    new XElement("title", "Temperature Data"),
                    new XElement("link",
                        new XAttribute("rel", "stylesheet"),
                        new XAttribute("type", "text/css"),
                        new XAttribute("href", "/static-files/style.css")
                    )
                )
            );
            var body = new XElement("body");
            var table = new XElement("table",
                new XElement("th", "DATUM"),
                new XElement("th", "TEMPERATUR"),
                new XElement("a",
                    new XAttribute("href",
                        "temp.html?from=" + url.Parameter["from"] +
                        "&until=" + url.Parameter["until"] + "&page=" +
                        (page - 1) + "&GetTemperature=")
                ), "previous | ",
                new XElement("a",
                    new XAttribute("href",
                        "temp.html?from=" + url.Parameter["from"] +
                        "&until=" + url.Parameter["until"] + "&page=" +
                        (page + 1) + "&GetTemperature=")
                ), "next",
                new XElement("br"),
                new XElement("a",
                    new XAttribute("href", "/static-files/index.html")
                ), "index.html"
            );
            var i = 0;
            if (page != 1)
            {
                i = (page - 1) * 20;
            }

            var current = 0;
            foreach (var (key, value) in data)
            {
                if (current < page * 20)
                {
                    current++;
                    continue;
                }

                if (i++ > page * 20)
                {
                    break;
                }

                var temp = new XElement("tr",
                    new XElement("td", key),
                    new XElement("td", value)
                );
                table.Add(temp);
            }

            body.Add(table);
            head.Add(body);
            xDocument.Add(head);

            return xDocument.ToString();
        }

        /// <summary>
        /// Parses a given string to a valid Datetime object, throws FormatException if string is not valid.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>A new Datetime object.</returns>
        /// <exception cref="FormatException">Throws FormatException if string is not valid.</exception>
        public DateTime ParseToDateTime(string s)
        {
            if (DateTime.TryParse(s, out var result))
            {
                return result;
            }

            throw new FormatException("Unable to Parse string to DateTime");
        }
    }
}