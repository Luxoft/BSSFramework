using Anch.Core;
using Anch.Threading;

using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing;

public class ServiceProviderPool : IServiceProviderPool
{
    private int mainIndex;

    private static int globalMainIndex;


    private bool disposed;

    private IServiceProviderPool? internalServiceProviderPool;

    public int MainIndex => this.mainIndex;

    public int GlobalMainIndex => globalMainIndex;

    public bool IsRoot { get; } = true;

    public object TestFramework { get; }

    public IServiceProviderPool? Inner => this.internalServiceProviderPool;

    private readonly AsyncLazy<IServiceProviderPool> lazyInternalServiceProviderPool;

    public ServiceProviderPool(ITestEnvironment testEnvironment, bool? allowParallelization, object testFramework)
    {
        this.TestFramework = testFramework;

        this.lazyInternalServiceProviderPool = new(async ct =>
        {
            Interlocked.Increment(ref this.mainIndex);

            Interlocked.Increment(ref globalMainIndex);

            var serviceProviderBuildContext = ServiceProviderBuildContext.Main;

            var services = new ServiceCollection()
                           .AddKeyedSingleton<IServiceProvider>(ITestEnvironment.MainServiceProviderKey, (sp, _) => sp)
                           .AddSingleton(serviceProviderBuildContext.Index)
                           .AddSingleton<IParallelizationSettings, ParallelizationSettings>()
                           .AddSingleton<IServiceProviderPool>(this);

            if (allowParallelization != null)
            {
                services.AddSingleton(new AllowParallelizationConstraint(allowParallelization.Value));
            }

            var mainServiceProvider = testEnvironment.BuildServiceProvider(services, serviceProviderBuildContext);

            foreach (var initializer in mainServiceProvider.GetKeyedServices<IInitializer>(ITestEnvironment.MainServiceProviderKey))
            {
                await initializer.Initialize(ct);
            }

            var mainServiceProviderSettings = mainServiceProvider.GetService<IMainServiceProviderSettings>();

            return new InternalServiceProviderPool(
                testEnvironment,
                mainServiceProvider,
                mainServiceProvider.GetRequiredService<IParallelizationSettings>(),
                mainServiceProviderSettings?.ReturnToPool ?? true,
                this);
        });


    }


    public async ValueTask<IServiceProvider> GetAsync(CancellationToken ct)
    {
        var v = await this.lazyInternalServiceProviderPool.GetValueAsync(ct);

        return await v.GetAsync(ct);
    }

    public async ValueTask ReleaseAsync(IServiceProvider serviceProvider, CancellationToken ct)
    {
        var v = await this.lazyInternalServiceProviderPool.GetValueAsync(ct);

        await v.ReleaseAsync(serviceProvider, ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref this.disposed, true))
        {
            return;
        }

        await using (this.internalServiceProviderPool) ;
    }
}
