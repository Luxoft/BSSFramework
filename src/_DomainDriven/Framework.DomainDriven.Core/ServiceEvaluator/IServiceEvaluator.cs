using SecuritySystem.Credential;

namespace Framework.DomainDriven;

public interface IServiceEvaluator<out TService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, UserCredential? userCredential, Func<TService, Task<TResult>> getResult);
}
