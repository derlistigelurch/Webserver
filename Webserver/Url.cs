using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Url : IUrl
    {
        public Url(string url)
        {
            this.RawUrl = url;
            this.ParameterCount = 0;
            this.Parameter = new Dictionary<string, string>();
            this.Path = "";
            this.Segments = new string[1];
            this.Segments[0] = "";
            this.FileName = "";
            this.Extension = "";
            this.Fragment = "";

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
                                string[] currentParameter = item.Split("=");
                                this.Parameter.Add(currentParameter.First(), currentParameter.Last());
                            }
                        }
                    }
                }

                // Segments
                string[] segmentStrings = this.Path.Split("/");
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

        public string RawUrl { get; }
        public string Path { get; }
        public IDictionary<string, string> Parameter { get; }
        public int ParameterCount { get; }
        public string[] Segments { get; }
        public string FileName { get; }
        public string Extension { get; }
        public string Fragment { get; }
    }
}