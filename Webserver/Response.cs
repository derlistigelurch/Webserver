using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Response : IResponse
    {
        public Response()
        {
            this.ServerHeader = "BIF-SWE1-Server";
            this.Headers = new Dictionary<string, string>();
        }

        private int _statusCode;
        public IDictionary<string, string> Headers { get; }
        public int ContentLength { get; }
        public string ContentType { get; set; }

        public int StatusCode
        {
            get
            {
                if (this._statusCode == 0)
                {
                    throw new System.InvalidOperationException("No status code set");
                }

                return this._statusCode;
            }

            set => this._statusCode = value;
        }

        public string Status
        {
            get
            {
                switch (this.StatusCode)
                {
                    case 200:
                        return "200 OK";
                    case 404:
                        return "404 NOT FOUND";
                    case 500:
                        return "500 INTERNAL SERVER ERROR";
                    default:
                        return null;
                }
            }
        }

        public void AddHeader(string header, string value)
        {
            this.Headers[header] = value;
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