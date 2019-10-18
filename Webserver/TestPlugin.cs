using System;
using System.Linq;
using System.Linq.Expressions;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class TestPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            if (req.Url.RawUrl.Contains("test") == false)
            {
                return 0.0f;
            }

            if (req.Url.Parameter.ContainsKey("test_plugin") == false)
            {
                return 0.4f;
            }

            if (bool.TryParse(req.Url.Parameter["test_plugin"], out bool canHandle))
            {
                return canHandle ? 1.0f : 0.0f;
            }

            return 0.6f;
        }

        public IResponse Handle(IRequest req)
        {
            return new Response();
        }
    }
}