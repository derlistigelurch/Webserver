using System.Collections.Generic;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class PluginManager : IPluginManager
    {
        public PluginManager()
        {
            
        }

        public IEnumerable<IPlugin> Plugins { get; }

        public void Add(IPlugin plugin)
        {
            throw new System.NotImplementedException();
        }

        public void Add(string plugin)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}