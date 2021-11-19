using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncHell
{
    internal class Program
    {
        public static async Task Main(string[] args) =>
            args.SelectMany(File.ReadAllLines).Select(ProcessLine);


        private static Task<int> ProcessLine(string line)
        {
            JustPrintToConsole(line, CancellationToken.None);
            int value = 0;
            try
            {
                value = ParseValue(line).Result;
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

            return Task.FromResult(value);
        }

        private static async void JustPrintToConsole(string line, CancellationToken ctoken)
        {
            await Task.Delay(100, ctoken);
            Console.WriteLine(line);
        }

        private static Task<int> ParseValue(string line) => Task.FromResult(int.Parse(line));

        private static async Task<int> AddOne(int value)
        {
            await Task.Delay(1);
            if (value <= 0) throw new ArgumentException(nameof(value), "Value must be greater than zero!");
            return value;
        }
    }
}
