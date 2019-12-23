using System;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    public class NaviPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            throw new NotImplementedException();
        }
    }
}