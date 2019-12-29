using System.IO;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    /// <summary>
    /// A Plugin which loads static files from a known directory.
    /// </summary>
    public class StaticFilePlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            // req? = if(req == null)
            return req?.Url == null
                ? 0.0f
                : 0.5f;
        }

        /// <summary>
        /// Called by the server when the plugin should handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            var response = new Response {StatusCode = 200};
            response.AddHeader("Connection", "close");

            // Check if the request files exists
            if (req.Url.Path.Equals("/"))
            {
                response.SetContent(File.ReadAllBytes(Path.Combine(System.Environment.CurrentDirectory,
                    Configuration.CurrentConfiguration.StaticFileDirectory, "index.html")));
                response.ContentType = GetMimeType(req.Url.Extension);
                return response;
            }

            if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, Path.Combine(req.Url.Segments))))
            {
                response.SetContent(
                    File.ReadAllBytes(Path.Combine(System.Environment.CurrentDirectory,
                        Path.Combine(req.Url.Segments))));
                response.ContentType = GetMimeType(req.Url.Extension);
                return response;
            }

            if (File.Exists(req.Url.Path))
            {
                response.SetContent(File.ReadAllBytes(req.Url.Path));
                response.ContentType = GetMimeType(req.Url.Extension);
                return response;
            }

            response.StatusCode = 404;
            return response;
        }

        /// <summary>
        /// Returns the correct Mimetype for a specific extension, never return null.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns>Correct Mime Type of a file.</returns>
        public string GetMimeType(string extension)
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
                case "json":
                    return "application/json";
                default:
                    return "";
            }
        }
    }
}