namespace Framework.DomainDriven.Jobs;

public interface IJobEvaluatorFactory
{
    IJobEvaluator Create(bool withRootLogging);

    Task RunService<TService>(Func<TService, Task> executeAsync)
        where TService : notnull => this.Create(false).RunService(executeAsync);
}
