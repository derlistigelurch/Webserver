using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    /// <summary>
    /// Parses an incoming HTTP Request.
    /// </summary>
    public class Request : IRequest
    {
        /// <summary>
        /// Parses an incoming HTTP Request.
        /// </summary>
        /// <param name="network"></param>
        public Request(System.IO.Stream network)
        {
            var streamReader = new StreamReader(network, Encoding.UTF8);
            this.Headers = new Dictionary<string, string>();

            string line;

            // if line is empty --> end of header
            while ((line = streamReader.ReadLine()) != null && string.IsNullOrEmpty(line) == false)
            {
                if (line.Contains(':'))
                {
                    var temp = line.Split(':');
                    this.Headers.Add(temp.First().Trim().ToLower(), temp.Last().Trim());
                }
                else
                {
                    // first line of http header
                    // GET /tutorials/other/top-20-mysql-best-practices/ HTTP/1.1
                    if (line.Contains(" "))
                    {
                        var temp = line.Split(" ");
                        if (temp.Length > 2)
                        {
                            this.Method = temp[0].ToUpper();
                            this.Url = new Url(temp[1]);
                        }
                        else
                        {
                            this.Method = temp.First().ToUpper();
                        }
                    }
                }
            }

            // POST /test HTTP/1.1
            // Host: foo.example
            // Content-Type: application/x-www-form-urlencoded
            // Content-Length: 27
            //
            // field1=value1&field2=value2 <-- CONTENT

            // read content
            if (this.ContentLength > 0)
            {
                // readBlock needs a character array
                var content = new char[this.ContentLength];
                // read the content stream
                streamReader.ReadBlock(content, 0, this.ContentLength);
                this.ContentString = new string(content);
                this.ContentBytes = Encoding.UTF8.GetBytes(content);
                this.ContentStream = new MemoryStream(this.ContentBytes);
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

        /// <summary>
        /// Returns true if the request is valid. A request is valid, if method and url could be parsed. A header is not necessary.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Returns the request method in UPPERCASE. get -> GET.
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// Returns a URL object of the request. Never returns null.
        /// </summary>
        public IUrl Url { get; }

        /// <summary>
        /// Returns the request header. Never returns null. All keys must be lower case.
        /// </summary>
        public IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Returns the user agent from the request header
        /// </summary>
        public string UserAgent => this.Headers["user-agent"];

        /// <summary>
        /// Returns the number of header or 0, if no header where found.
        /// </summary>
        public int HeaderCount => this.Headers.Count;

        /// <summary>
        /// Returns the parsed content length request header.
        /// </summary>
        public int ContentLength => this.Headers.ContainsKey("content-length")
            ? int.Parse(this.Headers["content-length"])
            : 0;

        /// <summary>
        /// Returns the parsed content type request header. Never returns null.
        /// </summary>
        public string ContentType => string.IsNullOrEmpty(this.Headers["content-type"])
            ? string.Empty
            : this.Headers["content-type"];

        /// <summary>
        /// Returns the request content (body) stream or null if there is no content stream.
        /// </summary>
        public Stream ContentStream { get; }

        /// <summary>
        /// Returns the request content (body) as string or null if there is no content.
        /// </summary>
        public string ContentString { get; }

        /// <summary>
        /// Returns the request content (body) as byte[] or null if there is no content.
        /// </summary>
        public byte[] ContentBytes { get; }
    }
}