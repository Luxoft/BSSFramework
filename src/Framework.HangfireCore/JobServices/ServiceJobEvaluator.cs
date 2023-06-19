using Framework.DomainDriven;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.HangfireCore.JobServices;

public class ServiceJobEvaluator<TService> : IServiceJobEvaluator<TService>
{
    private readonly IDBSessionEvaluator dbSessionEvaluator;

    public ServiceJobEvaluator(IDBSessionEvaluator dbSessionEvaluator)
    {
        this.dbSessionEvaluator = dbSessionEvaluator;
    }

    public Task<TResult> ExecuteAsync<TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> evaluate)
    {
        return this.dbSessionEvaluator.EvaluateAsync(
            sessionMode,
            (scopedSp, _) => scopedSp.GetRequiredService<IScopedJobExecutor>()
                                     .ExecuteAsync(
                                         () => evaluate(scopedSp.GetRequiredService<TService>())));
    }
}

public class ServiceJobEvaluator : IServiceJobEvaluator
{
    private readonly IServiceProvider rootServiceProvider;

    public ServiceJobEvaluator(IServiceProvider rootServiceProvider)
    {
        this.rootServiceProvider = rootServiceProvider;
    }

    public Task<TResult> ExecuteAsync<TService, TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> evaluate)
    {
        return this.rootServiceProvider.GetRequiredService<IServiceJobEvaluator<TService>>().ExecuteAsync(sessionMode, evaluate);
    }
}
