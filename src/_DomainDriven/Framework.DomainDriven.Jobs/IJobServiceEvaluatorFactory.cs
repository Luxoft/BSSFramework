namespace Framework.DomainDriven.Jobs;

public interface IJobServiceEvaluatorFactory
{
    Task RunJob<TJob>(CancellationToken cancellationToken = default)
        where TJob : IJob =>
        this.Create<TJob>(true).EvaluateAsync(job => job.ExecuteAsync(cancellationToken));

    IJobServiceEvaluator<TService> Create<TService>(bool withRootLogging)
        where TService : notnull;
}
