using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    /// <summary>
    /// A Plugin which is used to test the functionality of other classes
    /// </summary>
    public class TestPlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            if (req.Url == null)
            {
                return 0.0f;
            }

            if (req.Url.RawUrl.Equals("/"))
            {
                return 1.0f;
            }

            if (req.Url.Parameter.ContainsKey("test_plugin"))
            {
                if (bool.TryParse(req.Url.Parameter["test_plugin"], out var canHandle))
                {
                    return canHandle ? 1.0f : 0.0f;
                }
            }

            if (req.Url.Path.Contains("test"))
            {
                return 0.5f;
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
            var response = new Response
            {
                StatusCode = 200,
                ContentType = "text/html"
            };

            response.AddHeader("Connection", "close");
            response.SetContent("<html><body><h1>Hello World!</h1><p>testPlugin</p>" +
                                "<a href='/index.html'>index.html</a></body></html>");
            return response;
        }
    }
}