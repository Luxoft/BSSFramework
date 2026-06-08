using Anch.SecuritySystem;

using Framework.Database;

namespace Framework.BLL;

public interface ISyncServiceEvaluator<out TService>
{
    TResult Evaluate<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, TResult> getResult);
}
