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
            Url url = new Url("/hallo/welt/test.jpg?x=1&y=2#ffff");
            
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();

            Socket socket = listener.AcceptSocket();
            NetworkStream stream = new NetworkStream(socket);

            // test request
            Request request = new Request(stream);
            // test response
            Response response = new Response();
        }
    }
}