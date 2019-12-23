using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
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
            if (File.Exists(req.Url.Path))
            {
                response.SetContent(File.ReadAllText(req.Url.Path));
                return response;
            }

            response.StatusCode = 404;
            return response;
        }
    }
}