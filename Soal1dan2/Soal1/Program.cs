using System;
using System.Linq;

class Program
{
	static void Main(string[] args)
	{
		//Ubah parameter function sesuai yang kamu mau!
		Console.WriteLine(TransformInput("123456")); 
		Console.WriteLine(TransformInput("ABCDEF"));  
		Console.WriteLine(TransformInput("BA3&H-M")); 
	}

	static string TransformInput(string input)
	{
		int mid = input.Length / 2;
		bool isEven = input.Length % 2 == 0;

		string left = new string(input.Substring(0, mid).ToCharArray().Reverse().ToArray());
		
		string right = new string(input.Substring(isEven ? mid : mid + 1).ToCharArray().Reverse().ToArray());

		return left + (isEven ? "" : input[mid].ToString()) + right;
	}
}
