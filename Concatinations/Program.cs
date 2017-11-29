using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Concatinations
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Concatinating...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<string> allConcatinations = new List<string>();
            List<string> allWordsSortedByLength = GetWordsSortedByLength();
            int shortestLength = allWordsSortedByLength[allWordsSortedByLength.Count - 1].Length;
            HashSet<String> allWordsHash = new HashSet<String>(allWordsSortedByLength);

            foreach (string word in allWordsSortedByLength)
            {
                if (IsConcatination(word, word, allWordsHash, shortestLength))
                {
                    allConcatinations.Add(word);
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Concatination Completed in {stopwatch.ElapsedMilliseconds/1000.0} seconds.");
            Console.WriteLine($"There were {allConcatinations.Count} concatinated words");
            if (allConcatinations.Count > 0)
                Console.WriteLine($"The longest concatinated word was:{allConcatinations[0]}");
            if (allConcatinations.Count > 1)
                Console.WriteLine($"The second longest concatinated word was:{allConcatinations[1]}");

        }

        private static List<string> GetWordsSortedByLength()
        {
            string[] allWords =  File.ReadAllLines("AllWords.txt");
            return allWords.OrderByDescending(word => word.Length).Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        private static IEnumerable<string> GetSuffixes(string word, HashSet<string> allWordsHash)
        {
            var output = new List<string>();
            for (int i = 1; i < word.Length; i++)
            {
                if (allWordsHash.Contains(word.Substring(0, i)))
                    output.Add(word.Substring(i));
            }
            return output;
        }

        private static bool IsConcatination(string originalWord, string word, HashSet<string> allWordsHash, int shortestLength)
        {
            int length = word.Length;

            if (length < shortestLength)
                return false;

            if (length == shortestLength)
            {
                if (allWordsHash.Contains(word) & word != originalWord) return true;
                else return false;
            }

            foreach (string suffix in GetSuffixes(word, allWordsHash))
            {
                if (allWordsHash.Contains(suffix))
                {
                    return true;
                }

                if (IsConcatination(originalWord, suffix, allWordsHash, shortestLength))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
