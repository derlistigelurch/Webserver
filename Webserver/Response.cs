using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Response : IResponse
    {
        /// <summary>
        /// Creates a new HTTP response.
        /// </summary>
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

        /// <summary>
        /// Returns a writable dictionary of the response headers. Never returns null.
        /// </summary>
        public IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Returns the content length or 0 if no content is set yet.
        /// </summary>
        public int ContentLength
        {
            get
            {
                try
                {
                    return int.Parse(this.Headers["Content-Length"]);
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets or sets the content type of the response.
        /// </summary>
        public string ContentType
        {
            get => this.Headers["Content-Type"];
            set => this.Headers["Content-Type"] = value;
        }

        /// <summary>
        /// Gets or sets the current status code. An Exceptions is thrown, if no status code was set.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws an InvalidOperationException if statusCode is 0.</exception>
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

        /// <summary>
        /// Returns the status code as string. (200 OK)
        /// </summary>
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

        /// <summary>
        /// Adds or replaces a response header in the headers dictionary.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        public void AddHeader(string header, string value)
        {
            this.Headers[header] = value;
        }

        /// <summary>
        /// Gets or sets the Server response header. Defaults to "BIF-SWE1-Server".
        /// </summary>
        public string ServerHeader { get; set; }

        /// <summary>
        /// Sets a string content. The content will be encoded in UTF-8.
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(string content)
        {
            this.SetContent(Encoding.UTF8.GetBytes(content));
        }

        /// <summary>
        /// Sets a byte[] as content.
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(byte[] content)
        {
            this._content = content;
            this.Headers["Content-Length"] = this._content.Length.ToString();
        }

        /// <summary>
        /// Sets the stream as content.
        /// </summary>
        /// <param name="stream"></param>
        public void SetContent(Stream stream)
        {
            // Convert stream to byte array
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            // write byte array to stream
            this.SetContent(memoryStream.ToArray());
        }

        /// <summary>
        /// Sends the response to the network stream.
        /// </summary>
        /// <param name="network"></param>
        /// <exception cref="ArgumentNullException">throws new ArgumentNullException if stream is null</exception>
        /// <exception cref="InvalidOperationException">Throws InvalidOperationException if content type is set but no content is set.</exception>
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
            var streamWriter = new StreamWriter(network);
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
                    var binaryWriter = new BinaryWriter(network);
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