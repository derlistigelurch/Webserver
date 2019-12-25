using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    public class TestPlugin : IPlugin
    {
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

        public IResponse Handle(IRequest req)
        {
            Response response = new Response
            {
                StatusCode = 200,
                ContentType = "text/html"
            };

            response.AddHeader("Connection", "close");
            response.SetContent("<html><body><h1>Hello World!</h1><p>testPlugin</p></body></html>");
            return response;
        }
    }
}