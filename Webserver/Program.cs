using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using BIF.SWE1.Interfaces;
using System.Text;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            // test url class
            var url = new Url("/hallo/welt/test.jpg?x=1&y=2#ffff");

            var listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();
            
            while (true)
            {
                var socket = listener.AcceptSocket();
                var stream = new NetworkStream(socket);

                // test request
                var request = new Request(stream);
                // test response
                var response = new Response();
                response.Send(stream);
            }
        }
    }
}