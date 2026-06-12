namespace Framework.Application.Jobs;

public interface IJobServiceEvaluatorFactory
{
    Task RunJob<TJob>(CancellationToken ct)
        where TJob : IJob =>
        this.Create<TJob>(true).EvaluateAsync(job => job.ExecuteAsync(ct), ct);

    IJobServiceEvaluator<TService> Create<TService>(bool withRootLogging)
        where TService : notnull;
}
