using System.IO;
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
            return string.IsNullOrEmpty(req.ContentString) == false /*&& req.ContentString.Contains("lower")*/
                ? 0.8f
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

            var response = new Response(){StatusCode = 200};
            response.AddHeader("Connection", "close");

            // get content from contentstring (lower=halloWelt&submit=)
            // realContentString = halloWelt
            var realContentString = req.ContentString.Split('&').First().Substring(req.ContentString.IndexOf('=') + 1);

            var result = (string.IsNullOrEmpty(realContentString))
                ? "Bitte geben Sie einen Text ein"
                : realContentString.ToLower();

            if (File.Exists(Path.Combine(Configuration.CurrentConfiguration.StaticFileDirectory, "toLower.html")))
            {
                response.SetContent(File.ReadAllText(Path.Combine(
                    Configuration.CurrentConfiguration.StaticFileDirectory,
                    "toLower.html")).Replace("<pre id=\"result\"></pre>", result));
            }

            response.ContentType = "text/html";
            return response;
        }
    }
}