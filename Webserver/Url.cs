using System.Collections.Generic;
using System.Linq;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Url : IUrl
    {
        /// <summary>
        /// Creates new URL Object from a given URL
        /// </summary>
        /// <param name="url"></param>
        public Url(string url)
        {
            this.RawUrl = url;
            this.ParameterCount = 0;
            this.Parameter = new Dictionary<string, string>();
            this.Path = string.Empty;
            this.Segments = new string[1];
            this.Segments[0] = string.Empty;
            this.FileName = string.Empty;
            this.Extension = string.Empty;
            this.Fragment = string.Empty;

            // RawUrl: /hallo/welt/test.jpg?x=1&y=2#ffff
            // Path: /hallo/welt/test.jpg
            // Parameter: x=1, y=2
            // ParameterCount: 2
            // Segments: hallo, welt, test.jpg
            // FileName: test.jpg
            // Extension: jpg
            // Fragment: ffff

            if (string.IsNullOrEmpty(this.RawUrl) == false)
            {
                // Path
                this.Path = this.RawUrl;

                if (this.RawUrl.Contains("#") || this.RawUrl.Contains("?"))
                {
                    if (this.RawUrl.Contains("#"))
                    {
                        // Path
                        this.Path = RawUrl.Split("#").First();

                        // Fragment
                        this.Fragment = this.RawUrl.Split("#").Last();
                    }

                    if (this.RawUrl.Contains("?"))
                    {
                        // Path
                        this.Path = RawUrl.Split("?").First();

                        // ParameterCount
                        this.ParameterCount = this.RawUrl.Split("?").Last().Split("&").Length;

                        // Parameter
                        if (this.ParameterCount > 0)
                        {
                            string[] parameterStrings = this.RawUrl.Split("?").Last().Split("&");
                            if (parameterStrings.Last().Contains("#"))
                            {
                                string tempString = parameterStrings.Last().Split("#").First();
                                parameterStrings[this.ParameterCount - 1] = tempString;
                            }

                            foreach (var item in parameterStrings)
                            {
                                var currentParameter = item.Split("=");
                                this.Parameter.Add(currentParameter.First(), currentParameter.Last());
                            }
                        }
                    }
                }

                // Segments
                var segmentStrings = this.Path.Split("/");
                this.Segments = new string[segmentStrings.Length - 1];
                for (int i = 1; i < segmentStrings.Length; i++)
                {
                    this.Segments[i - 1] = segmentStrings[i];
                }

                if (this.Segments.Last().Contains("."))
                {
                    // FileName
                    this.FileName = this.Segments.Last();
                    // Extension
                    this.Extension = this.Segments.Last().Split(".").Last();
                }
            }
        }

        /// <summary>
        /// Returns the raw url.
        /// </summary>
        public string RawUrl { get; }

        /// <summary>
        /// Returns the path of the url, without parameter.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Returns a dictionary with the parameter of the url. Never returns null.
        /// </summary>
        public IDictionary<string, string> Parameter { get; }

        /// <summary>
        /// Returns the number of parameter of the url. Returns 0 if there are no parameter.
        /// </summary>
        public int ParameterCount { get; }

        /// <summary>
        /// Returns the segments of the url path. A segment is divided by '/' chars. Never returns null.
        /// </summary>
        public string[] Segments { get; }

        /// <summary>
        /// Returns the filename (with extension) of the url path. If the url contains no filename, a empty string is returned. Never returns null. A filename is present in the url, if the last segment contains a name with at least one dot.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Returns the extension of the url filename, including the leading dot. If the url contains no filename, a empty string is returned. Never returns null.
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Returns the url fragment. A fragment is the part after a '#' char at the end of the url. If the url contains no fragment, a empty string is returned. Never returns null.
        /// </summary>
        public string Fragment { get; }
    }
}