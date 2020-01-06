using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BIF.SWE1.Interfaces;
using Webserver.Database;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            // insert 10000 rows test data
            // databaseConnection.InsertTestData();
            var databaseConnection = new DatabaseConnection();
            // read sensor data every 5 minutes
            ThreadPool.QueueUserWorkItem(databaseConnection.ReadSensorData);

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
                ThreadPool.QueueUserWorkItem(HandleRequest, socket);
                // HandleRequest(socket);
            }
        }

        /// <summary>
        /// Handle incomming requests.
        /// </summary>
        /// <param name="clientSocket"></param>
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
            
            pluginManager.Clear();
            socket.Close();
        }
    }
}