using Framework.Application.Auth;
using Framework.Application.Middleware;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Application.Jobs;

public class JobMiddlewareFactory(IServiceProvider serviceProvider, ApplicationDefaultUserAuthenticationServiceSettings applicationDefaultUserAuthenticationServiceSettings, JobImpersonateData? jobImpersonateData = null) : IJobMiddlewareFactory
{
    public IScopedEvaluatorMiddleware Create<TJob>(bool withRootLogging) => this.GetMiddlewares<TJob>(withRootLogging).Aggregate();

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
