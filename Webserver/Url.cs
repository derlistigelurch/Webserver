using System;
using System.Collections.Generic;
using System.IO;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Url : IUrl
    {
        public Url(string url)
        {
            this.RawUrl = url;
            this.Path = string.IsNullOrEmpty(url) ? "" : url.Contains("?") ? url.Split("?")[0] : this.RawUrl;
            this.ParameterCount = string.IsNullOrEmpty(this.RawUrl) == false && this.RawUrl.Contains("?")
                ? this.RawUrl.Split("?")[1].Split("&").Length
                : 0;
            if (ParameterCount > 0 && string.IsNullOrEmpty(RawUrl) == false)
            {
                this.Parameter = new Dictionary<string, string>();
                string[] Parameter = RawUrl.Split("?")[1].Split("&");
                foreach (var item in Parameter)
                {
                    string[] temp = item.Split("=");
                    this.Parameter.Add(temp[0], temp[1]);
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