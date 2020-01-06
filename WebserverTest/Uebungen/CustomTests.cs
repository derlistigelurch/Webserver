using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using Webserver;
using Webserver.Database;

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

        public TempPlugin.TempPlugin GetTempPlugin()
        {
            return new TempPlugin.TempPlugin();
        }

        public StaticFilePlugin.StaticFilePlugin GetStaticFilePlugin()
        {
            return new StaticFilePlugin.StaticFilePlugin();
        }
    }
}