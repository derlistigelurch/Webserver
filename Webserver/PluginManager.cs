using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BIF.SWE1.Interfaces;
using Webserver.Plugins;

namespace Webserver
{
    public class PluginManager : IPluginManager
    {
        public PluginManager()
        {
            Add(new TestPlugin());
            Add(new StaticFilePlugin());
            Add(new LowerPlugin());
            Add(new NaviPlugin());
            Add(new TempPlugin());
        }


        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        public void Add(IPlugin plugin)
        {
            ((List<IPlugin>) this.Plugins).Add(plugin);
        }

        public void Add(string plugin)
        {
            // valid string: Webserver.TestPlugin
            // check if string ist null or empty
            if (string.IsNullOrEmpty(plugin))
            {
                throw new ArgumentNullException(plugin, "Plugin must not be null!");
            }

            Type type = Type.GetType(plugin);
            // throws an exception if type is empty or does not implement IPlugin
            if (type == null)
            {
                throw new ArgumentNullException("Plugin must not be null!");
            }

            if (typeof(TestPlugin).GetInterfaces().Contains(typeof(IPlugin)) == false)
            {
                throw new InvalidOperationException("Plugin must implement IPlugin!;");
            }

            // Add new plugin
            Add((IPlugin) Activator.CreateInstance(type));
        }

        public void Clear()
        {
            // not working
            // this.Plugins.ToList().Clear();
            // working
            ((List<IPlugin>) this.Plugins).Clear();
        }
    }
}