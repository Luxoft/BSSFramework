namespace Framework.Database;

public interface IDBSessionEvaluator
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, Task<TResult>> getResult);
}
