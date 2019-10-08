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
            Url url = new Url("/test/bild/yolo.jpg?x=9&foo=bar#ffff");
            Request request = new Request(GetValidRequestStream("/test/bild/yolo.jpg?x=9&foo=bar#ffff"));
        }


        public static Stream GetValidRequestStream(string url, string method = "GET", string host = "localhost",
            string[][] header = null, string body = null)
        {
            byte[] bodyBytes = null;
            if (body != null)
            {
                bodyBytes = Encoding.UTF8.GetBytes(body);
            }

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms, Encoding.ASCII);

            sw.WriteLine("{0} {1} HTTP/1.1", method, url);
            sw.WriteLine("Host: {0}", host);
            sw.WriteLine("Connection: keep-alive");
            sw.WriteLine("Accept: text/html,application/xhtml+xml");
            sw.WriteLine("User-Agent: Unit-Test-Agent/1.0 (The OS)");
            sw.WriteLine("Accept-Encoding: gzip,deflate,sdch");
            sw.WriteLine("Accept-Language: de-AT,de;q=0.8,en-US;q=0.6,en;q=0.4");
            if (bodyBytes != null)
            {
                sw.WriteLine(string.Format("Content-Length: {0}", bodyBytes.Length));
                sw.WriteLine("Content-Type: application/x-www-form-urlencoded");
            }

            if (header != null)
            {
                foreach (var h in header)
                {
                    sw.WriteLine(string.Format("{0}: {1}", h[0], h[1]));
                }
            }

            sw.WriteLine();

            if (bodyBytes != null)
            {
                sw.Flush();
                ms.Write(bodyBytes, 0, bodyBytes.Length);
            }

            sw.Flush();

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}