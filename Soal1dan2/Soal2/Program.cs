using System;
using System.Linq;

class Program
{
	static void Main(string[] args)
	{
		// Ubah input ini sesuai dengan yang anda mau!
		string[] firstWords = { "cinema", "host", "aba", "train" };
		string[] secondWords = { "iceman", "shot", "bab", "rain" };

		for (int i = 0; i < firstWords.Length; i++)
		{
			Console.Write(IsAnagram(firstWords[i], secondWords[i]) ? "1" : "0");
		}
	}

	static bool IsAnagram(string word1, string word2)
	{
		if (word1.Length != word2.Length)
		{
			return false;
		}

		var sortedWord1 = String.Concat(word1.OrderBy(c => c));
		var sortedWord2 = String.Concat(word2.OrderBy(c => c));

		return sortedWord1 == sortedWord2;
	}
}
