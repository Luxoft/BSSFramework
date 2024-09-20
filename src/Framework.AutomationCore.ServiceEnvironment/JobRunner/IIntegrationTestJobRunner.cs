using Framework.DomainDriven.Jobs;

namespace Automation.ServiceEnvironment;

public interface IIntegrationTestJobRunner
{
    Task RunJob<TJob>(CancellationToken cancellationToken = default)
        where TJob : IJob =>
        this.RunJob<TJob>(job => job.ExecuteAsync(cancellationToken));

    Task RunJob<TJob>(Func<TJob, Task> executeAsync);
}
