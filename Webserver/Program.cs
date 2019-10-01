using System;
using System.Linq;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            Url url = new Url("/foo/bar/test.jpg?x=2&y=3#test");
        }
    }
}