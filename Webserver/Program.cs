using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using BIF.SWE1.Interfaces;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Webserver.Plugins;
using System.Data.OleDb;
using Webserver.Database;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            // insert 10000 rows test data
            // databaseConnection.InsertTestData();
            DatabaseConnection databaseConnection = new DatabaseConnection();
            // read sensor data every 5 minutes
            // databaseConnection.ReadSensorData();
            // databaseConnection.SelectTemperatureRange(new DateTime(2014, 1, 1), new DateTime(2014, 1, 2));
            // Console.WriteLine("-----------------------------------");
            // databaseConnection.SelectTemperatureExact(new DateTime(2014, 1, 1));
            // Console.WriteLine("-----------------------------------");
            // databaseConnection.SelectTemperatureAll();
            StringBuilder result = new StringBuilder();
            result.Append("static-files/temp.html?");
            result.Append("from=").Append(new DateTime(2014, 1, 1).ToString("yyyy-MM-dd")).Append("&");
            result.Append("until=").Append(new DateTime(2014, 1, 2).ToString("yyyy-MM-dd")).Append("&");
            result.Append("GetTemperature=");
            
            Console.WriteLine(result);
            

            // configure static file directory
            Configuration.CurrentConfiguration.StaticFileDirectory = "static-files";
            // create static file directory
            Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory,
                Configuration.CurrentConfiguration.StaticFileDirectory));

            var listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();

            while (true)
            {
                var socket = listener.AcceptSocket();
                // var thread = new Thread(HandleRequest);
                // thread.Start(socket);
                // ThreadPool.QueueUserWorkItem(HandleRequest, socket);
                HandleRequest(socket);
            }
        }

        private static void HandleRequest(object clientSocket)
        {
            var socket = (Socket) clientSocket;
            var stream = new NetworkStream(socket);

            var request = new Request(stream);
            var pluginManager = new PluginManager();

            var canHandle = 0.0f;
            IPlugin handlePlugin = null;

            foreach (var plugin in pluginManager.Plugins)
            {
                if (canHandle < plugin.CanHandle(request))
                {
                    canHandle = plugin.CanHandle(request);
                    handlePlugin = plugin;
                }
            }

            if (handlePlugin != null)
            {
                var pluginResponse = handlePlugin.Handle(request);
                pluginResponse.Send(stream);
            }
            else
            {
                var response = new Response {StatusCode = 404};
                response.Send(stream);
            }

            socket.Close();
        }
    }
}