using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncHell
{
    [TestFixture]
    public class Test2
    {
        [Test]
        public async Task RunTestOK()
        {
            var writer = new StringWriter();
            var n = await ProcessLine("1", writer);
            Assert.AreEqual(2, n);
            Assert.AreEqual("1\r\n", writer.ToString());
        }
        [Test]
        public async Task RunTestNegative()
        {
            var writer = new StringWriter();
            var n = await ProcessLine("-1", writer);
            Assert.AreEqual(-1, n);
        }
        
        [Test]
        public async Task RunTestNaN()
        {
            var writer = new StringWriter();
            var n = await ProcessLine("not a number", writer);
            Assert.AreEqual(0, 0);
            Assert.AreEqual("Bad line not a number\r\n", writer.ToString());
        }

        private Task<int> ProcessLine(string line, TextWriter writer)
        {
            var cts = new CancellationTokenSource();
            JustPrintToConsole(line, writer, cts.Token);
            int value = 0;
            var parseValueTask = ParseValue(line);
            try
            {
                value = parseValueTask.Result;
            }
            catch (FormatException)
            {
                writer.WriteLine($"Bad line {line}");
            }

            try
            {
                value = AddOne(value).Result;
            }
            catch (ArgumentException e)
            {
                writer.WriteLine($"Bad value in {line}");
            }

            cts.Cancel();
            return Task.FromResult(value);
        }

        private async void JustPrintToConsole(string line, TextWriter writer, CancellationToken token)
        {
            await Task.Delay(100, token);
            await writer.WriteLineAsync(line);
        }

        private Task<int> ParseValue(string line) => Task.FromResult(int.Parse(line));

        private async Task<int> AddOne(int value)
        {
            await Task.Delay(1);
            if (value < 0) throw new ArgumentException(nameof(value), "Value must be positive!");
            return value + 1;
        }
    }
}
