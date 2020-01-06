using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using Webserver;

namespace Uebungen
{
    public class UEB3
    {
        public void HelloWorld()
        {
        }

        public IRequest GetRequest(System.IO.Stream network)
        {
            return new Request(network);
        }

        public IResponse GetResponse()
        {
            return new Response();
        }

        public IPlugin GetTestPlugin()
        {
            return new TestPlugin.TestPlugin();
        }
    }
}