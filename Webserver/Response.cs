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
            this.Headers = new Dictionary<string, string>
            {
                {"Content-Type", string.Empty},
                {"Content-Length", string.Empty}
            };
        }

        private int _statusCode;
        private byte[] _content;
        public IDictionary<string, string> Headers { get; }

        public int ContentLength
        {
            get
            {
                try
                {
                    return int.Parse(this.Headers["Content-Length"]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public string ContentType
        {
            get => this.Headers["Content-Type"];
            set => this.Headers["Content-Type"] = value;
        }

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
                        return "404 Not Found";
                    case 500:
                        return "500 Internal Server Error";
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
            this.SetContent(Encoding.UTF8.GetBytes(content));
        }

        public void SetContent(byte[] content)
        {
            this._content = content;
            this.Headers["Content-Length"] = this._content.Length.ToString();
        }

        public void SetContent(Stream stream)
        {
            // Convert stream to byte array
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            SetContent(memoryStream.ToArray());
        }

        public void Send(Stream network)
        {
            if (network == null)
            {
                throw new System.ArgumentNullException(nameof(network));
            }

            if (string.IsNullOrEmpty(this.ContentType) == false && this.ContentLength <= 0)
            {
                throw new System.InvalidOperationException("Setting a content type but no content is not allowed");
            }

            // Header
            StreamWriter streamWriter = new StreamWriter(network);
            streamWriter.WriteLine("HTTP/1.1 {0}", this.Status);
            streamWriter.WriteLine("Server: {0}", this.ServerHeader);

            foreach (var (key, value) in Headers)
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    streamWriter.WriteLine("{0}: {1}", key, value);
                }
            }

            streamWriter.WriteLine();
            // Clears all buffers for the current writer and causes any buffered data to be written to the underlying stream
            streamWriter.Flush();

            // Content
            if (this.StatusCode != 404 && this._content != null)
            {
                try
                {
                    BinaryWriter binaryWriter = new BinaryWriter(network);
                    binaryWriter.Write(this._content);
                    binaryWriter.Flush();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}