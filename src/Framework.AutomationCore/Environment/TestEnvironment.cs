using Automation.Utils;

namespace Automation;

public class TestEnvironment
{
    public readonly AssemblyInitializeAndCleanup AssemblyInitializeAndCleanup;

    public readonly IServiceProviderPool ServiceProviderPool;

    internal TestEnvironment(IServiceProviderPool serviceProviderPool, AssemblyInitializeAndCleanup assemblyInitializeAndCleanup)
    {
        AssemblyInitializeAndCleanup = assemblyInitializeAndCleanup;
        ServiceProviderPool = serviceProviderPool;
    }
}
