namespace Framework.DomainDriven;

public interface IServiceEvaluator<out TService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string? customPrincipalName, Func<TService, Task<TResult>> getResult);
}
