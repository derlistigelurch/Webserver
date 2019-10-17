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
            TcpListener listener = new TcpListener(IPAddress.Any, 8081);
            listener.Start();

            Socket socket = listener.AcceptSocket();
            NetworkStream stream = new NetworkStream(socket);

            Request request = new Request(stream);
            Response response = new Response();
        }
    }
}