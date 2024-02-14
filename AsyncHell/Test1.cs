using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncHell;

[TestFixture]
public class Test1
{
    [Test]
    public async Task RunTest([Values((object)new[] { "1.txt" })] string[] args)
    {
        var writer = new StringWriter();
        args.SelectMany(File.ReadAllLines).Select(writer.WriteLineAsync);
        Assert.AreEqual("line 1\r\n", writer.ToString());
    }
}
