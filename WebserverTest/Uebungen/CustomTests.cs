using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using Webserver;
using Webserver.Database;
using Webserver.Plugins;

namespace Uebungen
{
    public class CustomTests
    {
        public void HelloWorld()
        {
            // I'm fine
        }

        public DatabaseConnection GetDatabaseConnection()
        {
            return new DatabaseConnection();
        }

        public TempPlugin GetTempPlugin()
        {
            return new TempPlugin();
        }

        public StaticFilePlugin GetStaticFilePlugin()
        {
            return new StaticFilePlugin();
        }
    }
}