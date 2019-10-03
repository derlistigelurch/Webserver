using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Url url = new Url("/hallo/Welt/bild.jpg?x=7&y=foo#ffffff");
            
            var listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            var s = listener.AcceptSocket();
            var stream = new NetworkStream(s);
            var sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                Console.WriteLine(line);
            }
        }
    }
}