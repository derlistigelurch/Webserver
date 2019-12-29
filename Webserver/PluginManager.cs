using System;
using System.Collections.Generic;
using System.Linq;
using BIF.SWE1.Interfaces;
using Webserver.Plugins;

namespace Webserver
{
    /// <summary>
    /// Manages all Plugins.
    /// </summary>
    public class PluginManager : IPluginManager
    {
        /// <summary>
        /// Creates a new object to manage all plugins.
        /// Dynamically loads plugins.
        /// </summary>
        public PluginManager()
        {
            // Add("Webserver.Plugins.TestPlugin");
            Add(new TestPlugin());
            Add(new StaticFilePlugin());
            Add(new LowerPlugin());
            Add(new NaviPlugin());
            Add(new TempPlugin());
        }

        /// <summary>
        /// Returns a list of all plugins. Never returns null.
        /// </summary>
        public IEnumerable<IPlugin> Plugins { get; } = new List<IPlugin>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        public void Add(IPlugin plugin)
        {
            ((List<IPlugin>) this.Plugins).Add(plugin);
        }

        /// <summary>
        /// Adds a new plugin by type name. If the plugin was already added, nothing will happen.
        /// Throws an exeption, when the type cannot be resoled or the type does not implement IPlugin.
        /// </summary>
        /// <param name="plugin"></param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException if plugin is null.</exception>
        /// <exception cref="InvalidOperationException">Throws an InvalidOperationException if plugin does not implement IPlugin</exception>
        public void Add(string plugin)
        {
            // valid string: Webserver.TestPlugin
            // check if string ist null or empty
            if (string.IsNullOrEmpty(plugin))
            {
                throw new ArgumentNullException(plugin, "Plugin must not be null!");
            }

            var type = Type.GetType(plugin);
            // throws an exception if type is empty
            if (type == null)
            {
                throw new ArgumentNullException("Plugin must not be null!");
            }

            // throws exception if plugin does not implement IPlugin
            if (typeof(TestPlugin).GetInterfaces().Contains(typeof(IPlugin)) == false)
            {
                throw new InvalidOperationException("Plugin must implement IPlugin!;");
            }

            // Add new plugin
            Add((IPlugin) Activator.CreateInstance(type));
        }

        /// <summary>
        /// Clears all plugins
        /// </summary>
        public void Clear()
        {
            // not working
            // this.Plugins.ToList().Clear();
            // working
            ((List<IPlugin>) this.Plugins).Clear();
        }
    }
}