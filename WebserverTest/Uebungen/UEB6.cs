using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using Webserver;
using Webserver.Plugins;

namespace Uebungen
{
    public class UEB6
    {
        public void HelloWorld()
        {
        }

        public IPluginManager GetPluginManager()
        {
            return new PluginManager();
        }

        public IRequest GetRequest(System.IO.Stream network)
        {
            return new Request(network);
        }

        public string GetNaviUrl()
        {
            throw new NotImplementedException();
        }

        public IPlugin GetNavigationPlugin()
        {
            return new NaviPlugin();
        }

        public IPlugin GetTemperaturePlugin()
        {
            return new TempPlugin();
        }

        public string GetTemperatureRestUrl(DateTime from, DateTime until)
        {
            throw new NotImplementedException();
        }

        public string GetTemperatureUrl(DateTime from, DateTime until)
        {
            throw new NotImplementedException();
        }

        public IPlugin GetToLowerPlugin()
        {
            return new LowerPlugin();
        }

        public string GetToLowerUrl()
        {
            return "/static-files/toLower.html";
        }
    }
}