namespace Framework.DomainDriven.Jobs;

public interface IJobServiceEvaluator<out TService>
    where TService : notnull
{
    Task<TResult> EvaluateAsync<TResult>(Func<TService, Task<TResult>> executeAsync);
}
