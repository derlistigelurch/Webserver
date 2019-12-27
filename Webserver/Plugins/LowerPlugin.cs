using System;
using System.Linq;
using System.Text;
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
                response.SetContent("Bitte geben Sie einen Text ein");
                return response;
            }

            response.SetContent(realContentString.ToLower());
            return response;
        }
    }
}