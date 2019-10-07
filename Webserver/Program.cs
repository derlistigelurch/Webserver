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
        }
    }
}