using Framework.DomainDriven.Auth;
using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.DomainDriven.Jobs;

public class JobMiddlewareFactory(IServiceProvider serviceProvider, IApplicationDefaultUserAuthenticationServiceSettings applicationDefaultUserAuthenticationServiceSettings, JobImpersonateData? jobImpersonateData = null) : IJobMiddlewareFactory
{
    public IScopedEvaluatorMiddleware Create<TJob>(bool withRootLogging)
    {
        return this.GetMiddlewares<TJob>(withRootLogging).Aggregate();
    }

    protected virtual IEnumerable<IScopedEvaluatorMiddleware> GetMiddlewares<TService>(bool withRootLogging)
    {
        yield return new TryCloseSessionEvaluatorMiddleware(serviceProvider.GetRequiredService<IDBSessionManager>());

        yield return new ImpersonateEvaluatorMiddleware(serviceProvider, jobImpersonateData?.RunAs ?? applicationDefaultUserAuthenticationServiceSettings.DefaultValue);

        if (withRootLogging)
        {
            yield return new JobLoggingMiddleware<TService>(serviceProvider.GetRequiredService<ILogger<TService>>());
        }
    }
}
