using System.Collections.Concurrent;

namespace Automation;

public class ServiceProviderPool
{
    private readonly Func<IServiceProvider> createServiceProviderFunc;
    private readonly ConcurrentBag<IServiceProvider> providersCache = new ConcurrentBag<IServiceProvider>();
    private readonly SemaphoreSlim semaphore;

    internal ServiceProviderPool(Func<IServiceProvider> createServiceProviderFunc, bool parallelExecutionEnabled)
    {
        this.semaphore = parallelExecutionEnabled
            ? new SemaphoreSlim(Environment.ProcessorCount, Environment.ProcessorCount)
            : new SemaphoreSlim(1, 1);

        this.createServiceProviderFunc = createServiceProviderFunc;
    }

    public IServiceProvider Get()
    {
        this.semaphore.Wait();
        return this.providersCache.TryTake(out var provider) ? provider : this.createServiceProviderFunc.Invoke();
    }

    public void Release(IServiceProvider serviceProvider)
    {
        this.providersCache.Add(serviceProvider);
        this.semaphore.Release();
    }
}
