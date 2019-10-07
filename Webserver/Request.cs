using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Request : IRequest
    {
        public Request(System.IO.Stream network)
        {
            StreamReader streamReader = new StreamReader(network, Encoding.UTF8);
            this.Headers = new Dictionary<string, string>();

            int lineCount = 0;
            string line;

            while ((line = streamReader.ReadLine()) != null)
            {
                //if line is empty --> "end of header"
                if (string.IsNullOrEmpty(line) == false)
                {
                    if (line.Contains(':'))
                    {
                        string[] temp = line.Split(':');
                        this.Headers.Add(temp.First(), temp.Last());
                    }

                    // TODO: Parse first line of http header
                    /*
                     * GET /tutorials/other/top-20-mysql-best-practices/ HTTP/1.1
                     * Host: net.tutsplus.com
                     * User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)
                     * Accept: text/html,application/xhtml+xml,application/xml;q=0.9;q=0.8
                     * Accept-Language: en-us,en;q=0.5
                     * Accept-Encoding: gzip,deflate
                     * Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7
                     * Keep-Alive: 300
                     * Connection: keep-alive
                     * Cookie: PHPSESSID=r2t5uvjq435r4q7ib3vtdjq120
                     * Pragma: no-cache
                     * Cache-Control: no-cache
                     */
                }
            }
        }

        private void ParseHTTPHeader(string line)
        {
        }

        public bool IsValid { get; }
        public string Method { get; }
        public IUrl Url { get; }
        public IDictionary<string, string> Headers { get; }
        public string UserAgent { get; }
        public int HeaderCount { get; }
        public int ContentLength { get; }
        public string ContentType { get; }
        public Stream ContentStream { get; }
        public string ContentString { get; }
        public byte[] ContentBytes { get; }
    }
}