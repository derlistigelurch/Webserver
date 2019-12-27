using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    public class StaticFilePlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            if (req?.Url == null)
            {
                return 0.0f;
            }

            return 0.5f;
        }

        public IResponse Handle(IRequest req)
        {
            Response response = new Response {StatusCode = 200};
            response.AddHeader("Connection", "close");

            // Check if the request files exists
            if (req.Url.Path.Equals("/"))
            {
                response.SetContent(File.ReadAllBytes(Path.Combine(System.Environment.CurrentDirectory,
                    Configuration.CurrentConfiguration.StaticFileDirectory, "index.html")));
                response.ContentType = getMimeType(req.Url.Extension);
                return response;
            }

            if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, Path.Combine(req.Url.Segments))))
            {
                response.SetContent(
                    File.ReadAllBytes(Path.Combine(System.Environment.CurrentDirectory,
                        Path.Combine(req.Url.Segments))));
                response.ContentType = getMimeType(req.Url.Extension);
                return response;
            }

            if (File.Exists(req.Url.Path))
            {
                response.SetContent(File.ReadAllBytes(req.Url.Path));
                response.ContentType = getMimeType(req.Url.Extension);
                return response;
            }

            response.StatusCode = 404;
            return response;
        }

        private string getMimeType(string extension)
        {
            switch (extension)
            {
                case "html":
                    return "text/html";
                case "css":
                    return "text/css";
                case "js":
                    return "text/javascript";
                case "ico":
                    return "image/x-icon";
                case "txt":
                    return "text/plain";
                default:
                    return "";
            }
        }
    }
}