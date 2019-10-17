using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Request : IRequest
    {
        public Request(System.IO.Stream network)
        {
            StreamReader streamReader = new StreamReader(network, Encoding.ASCII);
            this.Headers = new Dictionary<string, string>();

            string line;

            while ((line = streamReader.ReadLine()) != null && string.IsNullOrEmpty(line) == false)
            {
                // if line is empty --> "end of header"
                if (line.Contains(':'))
                {
                    string[] temp = line.Split(':');
                    this.Headers.Add(temp[0].Trim().ToLower(), temp[1].Trim());
                }
                else
                {
                    // first line of http header
                    // GET /tutorials/other/top-20-mysql-best-practices/ HTTP/1.1
                    if (line.Contains(" "))
                    {
                        string[] temp = line.Split(" ");
                        if (temp.Length >= 2)
                        {
                            this.Method = temp[0].ToUpper();
                            this.Url = new Url(temp[1]);
                        }
                        else
                        {
                            this.Method = temp[0].ToUpper();
                        }
                    }
                }
            }

            // check if request is valid
            this.IsValid = true;
            if (string.IsNullOrEmpty(this.Method))
            {
                this.IsValid = false;
            }
            else if (!this.Method.Equals("GET") && !this.Method.Equals("POST"))
            {
                this.IsValid = false;
            }

            if (this.Url == null)
            {
                this.IsValid = false;
            }
        }

        public bool IsValid { get; }
        public string Method { get; }
        public IUrl Url { get; }
        public IDictionary<string, string> Headers { get; }
        public string UserAgent => Headers["user-agent"];
        public int HeaderCount => Headers.Count;
        public int ContentLength { get; }
        public string ContentType { get; }
        public Stream ContentStream { get; }
        public string ContentString { get; }
        public byte[] ContentBytes { get; }
    }
}