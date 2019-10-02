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
                string[] ParameterStrings = this.RawUrl.Split("?")[1].Split("&");
                foreach (var item in ParameterStrings)
                {
                    string[] temp = item.Split("=");
                    this.Parameter.Add(temp[0], temp[1]);
                }
            }

            if (string.IsNullOrEmpty(this.RawUrl) == false && this.RawUrl.Contains("#"))
            {
                this.Fragment = this.RawUrl.Split("#")[1];
            }
            else
            {
                this.Fragment = "";
            }

            if (string.IsNullOrEmpty(this.Path) == false)
            {
                string[] SegmentStrings = this.Path.Split("/");
                this.Segments = new string[SegmentStrings.Length - 1];
                for (int i = 1; i < SegmentStrings.Length; i++)
                {
                    this.Segments[i - 1] = SegmentStrings[i];
                }
            }
            else
            {
                this.Segments = new string[1];
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