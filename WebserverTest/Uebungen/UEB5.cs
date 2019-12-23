using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BIF.SWE1.Interfaces;
using Webserver;
using Webserver.Plugins;

namespace Uebungen
{
    public class UEB5
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

        public IPlugin GetStaticFilePlugin()
        {
            return new StaticFilePlugin();
        }

        public string GetStaticFileUrl(string fileName)
        {
            return Path.Combine(Configuration.CurrentConfiguration.StaticFileDirectory, fileName);
        }

        public void SetStaticFileFolder(string folder)
        {
            Configuration.CurrentConfiguration.StaticFileDirectory = folder;
        }
    }
}