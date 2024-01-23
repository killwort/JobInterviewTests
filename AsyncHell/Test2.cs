using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncHell
{
    [TestFixture]
    public class Test2
    {
        public async Task RunTest()
        {
            Task.WhenAll(DoStuffOneWay(), DoStuffAnotherWay());
        }

        private object _lockObject;
        private Mutex _resourceUsageMutex = new Mutex(false);

        public async Task DoStuffOneWay()
        {
            _resourceUsageMutex.WaitOne();
            //Do some work with exclusively owned resource
            await Task.Delay(1000);
            Monitor.Enter(_lockObject);
            //Do some work with some other resource
            await Task.Delay(1000);
            _resourceUsageMutex.ReleaseMutex();
            Monitor.Exit(_lockObject);
        }

        public async Task DoStuffAnotherWay()
        {
            Monitor.Enter(_lockObject);
            _resourceUsageMutex.WaitOne();
            try
            {
                //Do some work with both resources simultaneously 
                await Task.Delay(3333);
            }
            finally
            {
                _resourceUsageMutex.ReleaseMutex();
            }

            Monitor.Exit(_lockObject);
        }
    }
}
