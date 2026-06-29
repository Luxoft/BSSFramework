using Framework.Application.Jobs;
using Framework.Application.Middleware;

namespace Framework.Infrastructure.Hangfire;

public class MiddlewareJob<TJob, TArg>(
    TJob innerJob,
    JobInfo<TJob, TArg> jobInfo,
    IJobMiddlewareFactory jobMiddlewareFactory,
    ICancellationTokenResolver<TArg> cancellationTokenResolver)
{
    public Task ExecuteAsync(TArg arg) =>
        jobMiddlewareFactory.Create<TJob>(true).EvaluateAsync(
            async () => await jobInfo.ExecuteActon(innerJob, arg),
            cancellationTokenResolver.Resolve(arg));
}
