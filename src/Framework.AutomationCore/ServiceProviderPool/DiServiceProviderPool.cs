using System.Collections.Concurrent;

namespace Automation;

public class DiServiceProviderPool : IServiceProviderPool
{
    private readonly Func<IServiceProvider> createServiceProviderFunc;
    private readonly ConcurrentBag<IServiceProvider> providersCache = new ();

    public DiServiceProviderPool(
        ServiceProviderPoolFunc createServiceProviderFunc) =>
        this.createServiceProviderFunc = createServiceProviderFunc.Value;

    public IServiceProvider Get() =>
        this.providersCache.TryTake(out var provider) ? provider : this.createServiceProviderFunc.Invoke();

    public void Release(IServiceProvider serviceProvider) => this.providersCache.Add(serviceProvider);
}
