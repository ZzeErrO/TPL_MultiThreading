using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayrollUsingThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Employee Payroll using Threads");
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParallelTasks
            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task...");
                GetLongestWord(words);
            },
            () =>
            {
                Console.WriteLine("Begin second task...");
                GetMostCommonWords(words);
            },
            () =>
            {
                Console.WriteLine("Begin third task...");
                GetMostCommonWords(words);
            }//close third action
            );//close parallel.invoke
            #endregion
            Console.ReadKey();
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into q
                                 orderby q.Count() descending
                                 select q.Key;
            var commonWord = frequencyOrder.Take(10);
            foreach (var item in commonWord)
            {
                Console.WriteLine(item.ToString());
            }

        }

        public static string[] CreateWordArray(string url)
        {
            Console.WriteLine($"Retriving from {url}");
            //Download a web page the easy way
            string blog = new WebClient().DownloadString(url);
            return blog.Split(
                new char[] { ' ', '\u000A', '.', ',', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries
                );
        }

        private static string GetLongestWord(string[] words)
        {
            string longestWord = (from w in words
                                  orderby w.Length descending
                                  select w).First();
            Console.WriteLine($"Task 1 -- The Longest word is {longestWord}.");
            return longestWord;
        }
    }
}
