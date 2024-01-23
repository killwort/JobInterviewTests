using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncHell
{
    [TestFixture]
    public class Test1
    {
        [Test]
        public async Task RunTest(string[] args)
        {
            args.SelectMany(File.ReadAllLines).Select(ProcessLine);
        }

        private Task<int> ProcessLine(string line)
        {
            var cts = new CancellationTokenSource();
            JustPrintToConsole(line, cts.Token);
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

            cts.Cancel();
            return Task.FromResult(value);
        }

        private async void JustPrintToConsole(string line, CancellationToken ctoken)
        {
            await Task.Delay(100, ctoken);
            Console.WriteLine(line);
        }

        private Task<int> ParseValue(string line) => Task.FromResult(int.Parse(line));

        private async Task<int> AddOne(int value)
        {
            await Task.Delay(1);
            if (value <= 0) throw new ArgumentException(nameof(value), "Value must be greater than zero!");
            return value + 1;
        }
    }
}
