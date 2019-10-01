using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BIF.SWE1.Interfaces;

namespace Webserver
{
    public class Url : IUrl
    {
        public Url(string url)
        {
            this.RawUrl = url;
            //this.Path = string.IsNullOrEmpty(url) ? "" : url.Contains("?") ? url.Split("?")[0] : this.RawUrl;
            if (string.IsNullOrEmpty(this.RawUrl) == false)
            {
                if (this.RawUrl.Contains("#") || this.RawUrl.Contains("?"))
                {
                    if (this.RawUrl.Contains("#"))
                    {
                        this.Path = RawUrl.Split("#")[0];
                    }

                    if (this.RawUrl.Contains("?"))
                    {
                        this.Path = RawUrl.Split("?")[0];
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

            this.ParameterCount = string.IsNullOrEmpty(this.RawUrl) == false && this.RawUrl.Contains("?")
                ? this.RawUrl.Split("?")[1].Split("&").Length
                : 0;
            if (this.ParameterCount > 0 && string.IsNullOrEmpty(RawUrl) == false)
            {
                this.Parameter = new Dictionary<string, string>();
                string[] ParameterStrings = this.RawUrl.Split("?")[1].Split("&");
                foreach (var item in ParameterStrings)
                {
                    string[] temp = item.Split("=");
                    this.Parameter.Add(temp[0], temp[1]);
                }
            }

            this.Fragment = string.IsNullOrEmpty(this.RawUrl) || this.RawUrl.Contains("#") == false
                ? ""
                : this.RawUrl.Split("#")[1];
            
            if (string.IsNullOrEmpty(this.Path) == false)
            {
                string[] SegmentStrings = this.Path.Split("/");
                for (int i = 1; i < SegmentStrings.Length; i++)
                {
                    this.Segments[i] = new string[SegmentStrings[i]];
                }
            }
            else
            {
                this.Segments[0] = "";
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