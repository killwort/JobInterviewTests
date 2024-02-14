using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncHell
{
    [TestFixture]
    public class Test3
    {

        [Test]
        public async Task RunTest()
        {
            var task = IncrementAndReturnAsync();
            var r1 = await task;
            var rIntermediate = await IncrementAndReturnAsync();
            var r2 = await task;
            Assert.AreEqual(r1, r2);
            Assert.AreNotEqual(r1, rIntermediate);
        }

        private static int state;

        private async ValueTask<int> IncrementAndReturnAsync()
        {
            await Task.Yield();
            return Interlocked.Increment(ref state);
        }
    }
}
