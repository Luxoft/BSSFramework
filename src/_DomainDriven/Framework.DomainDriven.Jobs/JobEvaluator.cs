using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Jobs;

public class JobEvaluator(IServiceProvider rootServiceProvider) : IJobEvaluator
{
    public async Task RunJob<TJob>(Func<TJob, Task> executeAsync)
        where TJob : notnull
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var middlewareFactory = scope.ServiceProvider.GetRequiredService<IJobMiddlewareFactory>();

        var job = scope.ServiceProvider.GetRequiredService<TJob>();
        await middlewareFactory
              .Create<TJob>(true)
              .EvaluateAsync(async () => await executeAsync(job));
    }
}
