using System.Collections.Generic;
using System.IO;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Response : IResponse
    {
        public IDictionary<string, string> Headers { get; }
        public int ContentLength { get; }
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; }
        public void AddHeader(string header, string value)
        {
            throw new System.NotImplementedException();
        }

        public string ServerHeader { get; set; }
        public void SetContent(string content)
        {
            throw new System.NotImplementedException();
        }

        public void SetContent(byte[] content)
        {
            throw new System.NotImplementedException();
        }

        public void SetContent(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public void Send(Stream network)
        {
            throw new System.NotImplementedException();
        }
    }
}