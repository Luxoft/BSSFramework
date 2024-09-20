namespace Framework.DomainDriven.Jobs;

public interface IJobEvaluator
{
    Task RunJob<TJob>(CancellationToken cancellationToken = default)
        where TJob : IJob =>
        this.RunJob<TJob>(job => job.ExecuteAsync(cancellationToken));

    Task RunJob<TJob>(Func<TJob, Task> executeAsync)
        where TJob : notnull;
}
