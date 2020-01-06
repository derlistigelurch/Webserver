using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using Webserver;

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
            return "/static-files/navi.html";
        }

        public IPlugin GetNavigationPlugin()
        {
            return new NaviPlugin.NaviPlugin();
        }

        public IPlugin GetTemperaturePlugin()
        {
            return new TempPlugin.TempPlugin();
        }

        public string GetTemperatureRestUrl(DateTime from, DateTime until)
        {
            // /static-files/temp.html?from=2019-12-05&until=2019-12-26&type=rest&GetTemperature=
            StringBuilder result = new StringBuilder();
            result.Append("/static-files/temp.html?")
                .Append("from=").Append(from.ToString("yyyy-MM-dd")).Append("&")
                .Append("until=").Append(until.ToString("yyyy-MM-dd")).Append("&")
                .Append("type=rest&")
                .Append("GetTemperature=");
            return result.ToString();
        }

        public string GetTemperatureUrl(DateTime from, DateTime until)
        {
            //return "/static-files/temp.html?from=" + from + "&until=" + until + "&GetTemperature=";
            StringBuilder result = new StringBuilder();
            result.Append("/static-files/temp.html?")
                .Append("from=").Append(from.ToString("yyyy-MM-dd")).Append("&")
                .Append("until=").Append(until.ToString("yyyy-MM-dd")).Append("&")
                .Append("GetTemperature=");
            return result.ToString();
        }

        public IPlugin GetToLowerPlugin()
        {
            return new ToLowerPlugin.ToLowerPlugin.LowerPlugin();
        }

        public string GetToLowerUrl()
        {
            return "/static-files/toLower.html";
        }
    }
}