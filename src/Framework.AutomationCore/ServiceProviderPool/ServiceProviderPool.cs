using System.Collections.Concurrent;

namespace Automation;

public class ServiceProviderPool : IServiceProviderPool
{
    private readonly Func<IServiceProvider> createServiceProviderFunc;
    private readonly ConcurrentBag<IServiceProvider> providersCache = new ConcurrentBag<IServiceProvider>();
    private readonly SemaphoreSlim semaphore;
    private readonly bool enableLimiter;

    public ServiceProviderPool(Func<IServiceProvider> createServiceProviderFunc, bool parallelExecutionEnabled, bool disableLimiter)
    {
        this.enableLimiter = !disableLimiter;
        if (this.enableLimiter)
        {
            this.semaphore = parallelExecutionEnabled
                ? new SemaphoreSlim(Environment.ProcessorCount, Environment.ProcessorCount)
                : new SemaphoreSlim(1, 1);
        }

        this.createServiceProviderFunc = createServiceProviderFunc;
    }

    public IServiceProvider Get()
    {
        if (this.enableLimiter)
        {
            this.semaphore.Wait();
        }

        try
        {
            return this.providersCache.TryTake(out var provider) ? provider : this.createServiceProviderFunc.Invoke();
        }
        catch (Exception)
        {
            if (this.enableLimiter)
            {
                this.semaphore.Release();
            }

            throw;
        }

    }

    public void Release(IServiceProvider serviceProvider)
    {
        this.providersCache.Add(serviceProvider);
        if (this.enableLimiter)
        {
            this.semaphore.Release();
        }
    }
}
