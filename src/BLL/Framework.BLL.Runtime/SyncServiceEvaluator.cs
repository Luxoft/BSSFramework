using Anch.Core;
using Anch.SecuritySystem;

using Framework.Application;
using Framework.Database;

namespace Framework.BLL;

public class SyncServiceEvaluator<TService>(
    IServiceEvaluator<TService> serviceEvaluator,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : ISyncServiceEvaluator<TService>
{
    public TResult Evaluate<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, TResult> getResult)
    {
        TaskResultHelper<TResult>.TypeIsNotTaskValidate();

        return defaultCancellationTokenSource.RunSync(ct => serviceEvaluator.EvaluateAsync(
                                                          sessionMode,
                                                          customUserCredential,
                                                          service => Task.FromResult(getResult(service)),
                                                          ct));
    }
}
