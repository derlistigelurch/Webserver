using System.Collections.Generic;
using System.IO;
using BIF.SWE1.Interfaces;

namespace Webserver
{
	public class Url : IUrl
	{
		public Url(string url)
		{
			this.Path = url;
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