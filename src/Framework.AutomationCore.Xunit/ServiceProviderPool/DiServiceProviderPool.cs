using System.Collections.Concurrent;

using Bss.Testing.Xunit.Interfaces;

namespace Automation.Xunit.ServiceProviderPool;

public class DiServiceProviderPool(ServiceProviderPoolFunc createServiceProviderFunc) : ITestServiceProviderPool
{
    private readonly Func<IServiceProvider> createServiceProviderFunc = createServiceProviderFunc.Value;
    private readonly ConcurrentBag<IServiceProvider> providersCache = new ();

    public IServiceProvider Get() =>
        this.providersCache.TryTake(out var provider) ? provider : this.createServiceProviderFunc.Invoke();

    public void Release(IServiceProvider serviceProvider) => this.providersCache.Add(serviceProvider);
}
