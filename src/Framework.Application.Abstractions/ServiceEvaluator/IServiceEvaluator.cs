using Framework.Application.Session;

using SecuritySystem.Credential;

namespace Framework.Application.ServiceEvaluator;

public interface IServiceEvaluator<out TService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, UserCredential? userCredential, Func<TService, Task<TResult>> getResult);
}
