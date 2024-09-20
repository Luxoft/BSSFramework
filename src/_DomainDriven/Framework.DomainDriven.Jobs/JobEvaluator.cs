using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Jobs;

public class JobEvaluator(IServiceProvider rootServiceProvider, JobEvaluatorSettings settings) : IJobEvaluator
{
    public async Task RunService<TService>(Func<TService, Task> executeAsync)
        where TService : notnull
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var middlewareFactory = scope.ServiceProvider.GetRequiredService<IJobMiddlewareFactory>();

        var service = scope.ServiceProvider.GetRequiredService<TService>();

        await middlewareFactory
              .Create<TService>(settings.WithRootLogging)
              .EvaluateAsync(async () => await executeAsync(service));
    }
}
