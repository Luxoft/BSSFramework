namespace Framework.DomainDriven.Jobs;

public interface IJobEvaluator
{
    Task RunJob<TJob>(CancellationToken cancellationToken = default)
        where TJob : IJob =>
        this.RunService<TJob>(job => job.ExecuteAsync(cancellationToken));

    Task RunService<TService>(Func<TService, Task> executeAsync)
        where TService : notnull;
}
