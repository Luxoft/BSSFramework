using Anch.SecuritySystem;

using Framework.Database;

namespace Framework.Application;

public interface IServiceEvaluator<out TService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, UserCredential? userCredential, Func<TService, Task<TResult>> getResult);
}

