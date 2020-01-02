using System.Linq;
using System.Xml.Linq;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    /// <summary>
    /// A plugin which lowers text.
    /// </summary>
    public class LowerPlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            // POST /test HTTP/1.1
            // Host: foo.example
            // Content-Type: application/x-www-form-urlencoded
            // Content-Length: 15
            //
            // lower=halloWelt <-- CONTENT
            return string.IsNullOrEmpty(req.ContentString) == false
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
            // Post content string
            // lower=halloWelt
            // hallowelt

            var response = new Response {StatusCode = 200};
            response.AddHeader("Connection", "close");

            // get content from contentstring (lower=halloWelt&submit=)
            // realContentString = halloWelt
            var realContentString = req.ContentString.Split('&').First().Substring(req.ContentString.IndexOf('=') + 1);

            response.SetContent(string.IsNullOrEmpty(realContentString)
                ? CreateToLowerHtml("Bitte geben Sie einen Text ein")
                : CreateToLowerHtml(realContentString.ToLower()));

            response.ContentType = "text/html";
            return response;
        }

        /// <summary>
        /// Creates a valid HTML site.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>A valid HTML site</returns>
        private string CreateToLowerHtml(string result)
        {
            var xDocument = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XAttribute("lang", "de"),
                    new XElement("head"),
                    new XElement("title", "ToLower"),
                    new XElement("link",
                        new XAttribute("rel", "stylesheet"),
                        new XAttribute("type", "text/css"),
                        new XAttribute("href", "/static-files/style.css")
                    ),
                    new XElement("body",
                        new XElement("form",
                            new XAttribute("method", "post"),
                            new XElement("label",
                                new XAttribute("for", "lower"), "Input: ",
                                new XElement("input",
                                    new XAttribute("type", "text"),
                                    new XAttribute("id", "lower"),
                                    new XAttribute("name", "lower")
                                )),
                            new XElement("button",
                                new XAttribute("type", "submit"),
                                new XAttribute("name", "submit"), "Submit"
                            )
                        ),
                        new XElement("p", result),
                        new XElement("a",
                            new XAttribute("href", "/static-files/index.html")
                        ), "index.html"
                    )
                )
            );

            return xDocument.ToString();
        }
    }
}