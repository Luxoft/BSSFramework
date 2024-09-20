using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.DomainDriven.Jobs;

public class JobMiddlewareFactory(IServiceProvider serviceProvider) : IJobMiddlewareFactory
{
    public IScopedEvaluatorMiddleware Create<TJob>(string? runAs)
    {
        return this.GetMiddlewares<TJob>(runAs).Aggregate();
    }

    protected virtual IEnumerable<IScopedEvaluatorMiddleware> GetMiddlewares<TJob>(string? runAs)
    {
        yield return new JobLoggingMiddleware<TJob>(serviceProvider.GetRequiredService<ILogger<TJob>>());

        if (runAs != null)
        {
            yield return new ImpersonateEvaluatorMiddleware(serviceProvider, runAs);
        }

        yield return new TryCloseSessionEvaluatorMiddleware(serviceProvider.GetRequiredService<IDBSessionManager>());
    }
}
