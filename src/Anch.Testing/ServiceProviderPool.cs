using Anch.Core;
using Anch.Threading;

using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing;

public class ServiceProviderPool(ITestEnvironment testEnvironment, bool? allowParallelization, object testFramework) : IServiceProviderPool
{
    private int mainIndex;

    private static int globalMainIndex;


    private bool disposed;

    private readonly IAsyncLocker asyncLocker = new AsyncLocker();

    private IServiceProviderPool? internalServiceProviderPool;

    public int MainIndex => this.mainIndex;

    public IAsyncLocker AsyncLocker => this.asyncLocker;

    public int GlobalMainIndex => globalMainIndex;

    public bool IsRoot { get; } = true;

    public object TestFramework { get; } = testFramework;

    public IServiceProviderPool? Inner => this.internalServiceProviderPool;

    public async ValueTask<IServiceProvider> GetAsync(CancellationToken ct)
    {
        var v = await this.GetInternalServiceProviderPool(ct);

        return await v.GetAsync(ct);
    }

    public async ValueTask ReleaseAsync(IServiceProvider serviceProvider, CancellationToken ct)
    {
        var v = await this.GetInternalServiceProviderPool(ct);

        await v.ReleaseAsync(serviceProvider, ct);
    }

    private async ValueTask<IServiceProviderPool> GetInternalServiceProviderPool(CancellationToken ct)
    {
        if (this.internalServiceProviderPool == null)
        {
            using (await this.asyncLocker.CreateScope(ct))
            {
                if (this.internalServiceProviderPool == null)
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

                    this.internalServiceProviderPool = new InternalServiceProviderPool(
                        testEnvironment,
                        mainServiceProvider,
                        mainServiceProvider.GetRequiredService<IParallelizationSettings>(),
                        mainServiceProviderSettings?.ReturnToPool ?? true,
                        this);
                }
            }
        }

        return this.internalServiceProviderPool;
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
