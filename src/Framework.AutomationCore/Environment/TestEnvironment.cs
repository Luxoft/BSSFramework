namespace Automation;

public class TestEnvironment
{
    public readonly AssemblyInitializeAndCleanup AssemblyInitializeAndCleanup;

    public readonly IServiceProviderPool ServiceProviderPool;

    internal TestEnvironment(IServiceProviderPool serviceProviderPool, AssemblyInitializeAndCleanup assemblyInitializeAndCleanup)
    {
        this.AssemblyInitializeAndCleanup = assemblyInitializeAndCleanup;
        this.ServiceProviderPool = serviceProviderPool;
    }
}
