using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.ScopedEvaluate;

namespace Framework.HangfireCore;

public class MiddlewareJob<TJob, TArg>(TJob innerJob, JobInfo<TJob, TArg> jobInfo, IJobMiddlewareFactory jobMiddlewareFactory)
{
    public async Task ExecuteAsync(TArg arg)
    {
        await jobMiddlewareFactory
              .Create<TJob>(jobInfo.RunAs)
              .EvaluateAsync(async () => await jobInfo.ExecuteActon(innerJob, arg));
    }
}
