using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    public class LowerPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            // POST /test HTTP/1.1
            // Host: foo.example
            // Content-Type: application/x-www-form-urlencoded
            // Content-Length: 15
            //
            // lower=halloWelt <-- CONTENT
            if (string.IsNullOrEmpty(req.ContentString) == false)
            {
                return 1.0f;
            }

            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            // Post content string
            // lower=halloWelt
            // hallowelt

            Response response = new Response {StatusCode = 200};
            response.AddHeader("Connection", "close");

            // get content from contentstring (lower=halloWelt&submit=)
            // realContentString = halloWelt
            string realContentString =
                req.ContentString.Split('&').First().Substring(req.ContentString.IndexOf('=') + 1);
            if (string.IsNullOrEmpty(realContentString))
            {
                response.SetContent(CreateToLowerHtml("Bitte geben Sie einen Text ein"));
            }
            else
            {
                response.SetContent(CreateToLowerHtml(realContentString.ToLower()));
            }

            response.ContentType = "text/html";
            return response;
        }

        private string CreateToLowerHtml(string result)
        {
            XDocument xDocument = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XAttribute("lang", "de"),
                    new XElement("head"),
                    new XElement("title", "ToLower"),
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
                        new XElement("p", result)
                    )
                )
            );

            return xDocument.ToString();
        }
    }
}