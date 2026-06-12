using Framework.Application.Jobs;
using Framework.Application.Middleware;

namespace Framework.Infrastructure.Hangfire;

public class CancellationMiddlewareJob<TJob>(TJob innerJob, JobInfo<TJob, CancellationToken> jobInfo, IJobMiddlewareFactory jobMiddlewareFactory)
{
    public Task ExecuteAsync(CancellationToken ct) =>
        jobMiddlewareFactory.Create<TJob>(true).EvaluateAsync(async () => await jobInfo.ExecuteActon(innerJob, ct), ct);
}
