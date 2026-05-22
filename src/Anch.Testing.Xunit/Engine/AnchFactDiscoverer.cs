using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchFactDiscoverer : FactDiscoverer
{
    public override ValueTask<IReadOnlyCollection<IXunitTestCase>> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        IXunitTestMethod testMethod,
        IFactAttribute factAttribute)
    {
        if (testMethod.Method.LastParameterIsCt())
        {
            return new([this.CreateTestCase(discoveryOptions, testMethod, factAttribute)]);
        }
        else
        {
            return base.Discover(discoveryOptions, testMethod, factAttribute);
        }
    }
}