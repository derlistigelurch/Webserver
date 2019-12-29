using System;
using BIF.SWE1.Interfaces;

namespace Webserver.Plugins
{
    /// <summary>
    /// A Plugin which shows streets in a given area
    /// </summary>
    public class NaviPlugin : IPlugin
    {
        /// <summary>
        /// Returns a score between 0 and 1 to indicate that the plugin is willing to handle the request. The plugin with the highest score will execute the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A score between 0 and 1</returns>
        public float CanHandle(IRequest req)
        {
            return 0.0f;
        }

        /// <summary>
        /// Called by the server when the plugin should handle the request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>A new response object.</returns>
        public IResponse Handle(IRequest req)
        {
            throw new NotImplementedException();
        }
    }
}