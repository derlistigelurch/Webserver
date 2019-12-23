using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using BIF.SWE1.Interfaces;
using System.Text;
using System.Threading;
using Webserver.Plugins;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();

            // Url url = new Url("/test");
            while (true)
            {
                // var config = new Config() {StaticFileDirectory = "res"};
                // Directory.CreateDirectory(config.StaticFileDirectory);
                // Console.WriteLine(Path.Combine(config.StaticFileDirectory, "test.tx"));
                // if (File.Exists(Path.Combine(config.StaticFileDirectory, "test.txt")))
                // {
                //     Console.Write("hallo Welt");
                // }
                Console.WriteLine("HelloWorld");
            }
        }

        private static void HandleRequest(object clientSocket)
        {
            var socket = (Socket) clientSocket;
            var stream = new NetworkStream(socket);

            var request = new Request(stream);

            var testPlugin = new TestPlugin();
            if (testPlugin.CanHandle(request) > 0.0f)
            {
                var pluginResponse = testPlugin.Handle(request);
                // if(pluginResponse != null)
                pluginResponse?.Send(stream);
            }

            socket.Close();
        }
    }
}