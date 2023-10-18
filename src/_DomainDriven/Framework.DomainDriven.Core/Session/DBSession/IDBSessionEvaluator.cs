namespace Framework.DomainDriven;

public interface IDBSessionEvaluator
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, Task<TResult>> getResult);
}
