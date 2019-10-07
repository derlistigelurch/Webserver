using System;
using System.Collections.Generic;
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

            if (string.IsNullOrEmpty(this.RawUrl) == false)
            {
                if (this.RawUrl.Contains("#") || this.RawUrl.Contains("?"))
                {
                    if (this.RawUrl.Contains("#"))
                    {
                        this.Path = RawUrl.Split("#").First();
                    }

                    if (this.RawUrl.Contains("?"))
                    {
                        this.Path = RawUrl.Split("?").First();
                    }
                }
                else
                {
                    this.Path = this.RawUrl;
                }
            }
            else
            {
                this.Path = "";
            }

            if (string.IsNullOrEmpty(this.RawUrl) == false && this.RawUrl.Contains("?"))
            {
                this.ParameterCount = this.RawUrl.Split("?")[1].Split("&").Length;
            }
            else
            {
                this.ParameterCount = 0;
            }

            this.Parameter = new Dictionary<string, string>();
            if (this.ParameterCount > 0 && string.IsNullOrEmpty(RawUrl) == false)
            {
                string[] parameterStrings = this.RawUrl.Split("?").Last().Split("&");
                if (parameterStrings.Last().Contains("#"))
                {
                    string tempString = parameterStrings.Last().Split("#").First();
                    parameterStrings[ParameterCount - 1] = tempString;
                }

                foreach (var item in parameterStrings)
                {
                    string[] temp = item.Split("=");
                    this.Parameter.Add(temp.First(), temp.Last());
                }
            }

            if (string.IsNullOrEmpty(this.RawUrl) == false && this.RawUrl.Contains("#"))
            {
                this.Fragment = this.RawUrl.Split("#").Last();
            }
            else
            {
                this.Fragment = "";
            }

            if (string.IsNullOrEmpty(this.Path) == false)
            {
                string[] segmentStrings = this.Path.Split("/");
                this.Segments = new string[segmentStrings.Length - 1];
                for (int i = 1; i < segmentStrings.Length; i++)
                {
                    this.Segments[i - 1] = segmentStrings[i];
                }
            }
            else
            {
                this.Segments = new string[1];
                this.Segments[0] = "";
            }

            if (string.IsNullOrEmpty(this.Path) == false && (this.Path.Split("/").Last().Contains(".")))
            {
                this.FileName = this.Path.Split("/").Last();
                this.Extension = this.Path.Split(".").Last();
            }
            else
            {
                this.FileName = "";
                this.Extension = "";
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