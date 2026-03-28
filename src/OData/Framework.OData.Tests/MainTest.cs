using NUnit.Framework;

namespace Framework.OData.Tests;

[TestFixture]
public class MainTest
{
    [Test]
    [Ignore("Used for local hand running")]
    public void TestODataParse()
    {
        var request = File.ReadAllText("request.odata");

        var zzz = SelectOperation.Parse(request);

        var r = zzz.Filter.ToString();
    }
}
