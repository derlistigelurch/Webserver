using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using BIF.SWE1.Interfaces;
using Webserver.Database;

namespace Webserver.Plugins
{
    public class TempPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            if (req?.Url != null && req.Url.Parameter.ContainsKey("GetTemperature"))
            {
                return 1.0f;
            }

            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response() {StatusCode = 200};
            DatabaseConnection databaseConnection = new DatabaseConnection();
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

            if (req.Url.Parameter.ContainsKey("type") && req.Url.Parameter["type"].Equals("rest"))
            {
                response.ContentType = "text/xml";
                response.SetContent(CreateRestNaviXml(tempData));
            }
            else
            {
                response.ContentType = "text/html";
                response.SetContent(CreateNaviHtml(tempData));
            }

            return response;
        }

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

        public string CreateNaviHtml(Dictionary<string, double> data)
        {
            // Setup base structure:
            XDocument xDocument = new XDocument();
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
                    new XAttribute("href", "/index.html")
                ), "index.html"
            );
            foreach (var (key, value) in data)
            {
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

        public DateTime ParseToDateTime(string s)
        {
            if (DateTime.TryParse(s, out DateTime result))
            {
                return DateTime.Parse(s);
            }

            throw new FormatException("Unable to Parse string to DateTime");
        }
    }
}