using System;

namespace Webserver
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			String MyString = "/test.jpg?x=y?y=foo";
			
			// split string
			String[] Elements = MyString.Split('?');
			int ParamCount = Elements.Length - 1;
			
			foreach (var strings in Elements)
			{
				//first string = path
				//second ... parameter
				Console.WriteLine(strings);
			}
			
			//return parameter count
			Console.WriteLine(ParamCount);
			
			//return raw url
			Console.WriteLine(MyString);
			
		}
	}
}