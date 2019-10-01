using System.Collections.Generic;
using System.IO;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Request : IRequest
    {
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