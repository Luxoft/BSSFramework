using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Jobs;

public class JobServiceEvaluator<TService>(IServiceProvider rootServiceProvider, JobServiceEvaluatorSettings? settings = null)
    : IJobServiceEvaluator<TService>
    where TService : notnull
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<TService, Task<TResult>> executeAsync)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var middlewareFactory = scope.ServiceProvider.GetRequiredService<IJobMiddlewareFactory>();

        var service = scope.ServiceProvider.GetRequiredService<TService>();

        return await middlewareFactory
                     .Create<TService>(settings?.WithRootLogging ?? false)
                     .EvaluateAsync(async () => await executeAsync(service));
    }
}
