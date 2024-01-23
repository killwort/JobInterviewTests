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
        private Semaphore _semaphore = new Semaphore(0, 10);
        private int _completedItems;
        [Test]
        public async Task RunTest()
        {
            await Task.WhenAll(Enumerable.Range(0, 1000).Select(DoWorkInParallel));
            Assert.AreEqual(1000, _completedItems);
        }

        private async Task DoWorkInParallel(int i)
        {
            _semaphore.WaitOne();
            try
            {
                await Task.Delay(100).ConfigureAwait(false);
                _completedItems++;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
