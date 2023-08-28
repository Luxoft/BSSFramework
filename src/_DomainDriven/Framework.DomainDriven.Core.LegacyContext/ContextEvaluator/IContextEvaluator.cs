namespace Framework.DomainDriven;

public interface IContextEvaluator<out TBLLContext>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, IDBSession, Task<TResult>> getResult);
}
