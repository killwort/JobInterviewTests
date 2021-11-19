using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncHell
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            args.SelectMany(File.ReadAllLines).Select(ProcessLine);
        }

        private static async Task<int> ProcessLine(string line)
        {
            JustPrintToConsole(line);
            int value = 0;
            try
            {
                value = await ParseValue(line);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Bad line {line}");
            }

            try
            {
                value = AddOne(value).Result;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Bad value in {line}");
            }

            return value;
        }

        private static async void JustPrintToConsole(string line)
        {
            Console.WriteLine(line);
        }

        private static Task<int> ParseValue(string line) => Task.FromResult(int.Parse(line));

        private static Task<int> AddOne(int value)
        {
            if (value <= 0) throw new ArgumentException(nameof(value), "Value must be greater than zero!");
            return Task.FromResult(value);
        }
    }
}
