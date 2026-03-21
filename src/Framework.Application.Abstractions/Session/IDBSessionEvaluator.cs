namespace Framework.Application.Session;

public interface IDBSessionEvaluator
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, Task<TResult>> getResult);
}
