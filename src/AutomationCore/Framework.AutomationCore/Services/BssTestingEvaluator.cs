using Anch.Core;
using Anch.SecuritySystem;
using Anch.SecuritySystem.Testing;

using Framework.Application;
using Framework.Database;

namespace Framework.AutomationCore.Services;

public class BssTestingEvaluator<TService>(IServiceEvaluator<TService> serviceEvaluator, IDefaultCancellationTokenSource? defaultCancellationTokenSource = null)
    : ITestingEvaluator<TService>
    where TService : notnull
{
    public Task<TResult> EvaluateAsync<TResult>(TestingScopeMode mode, UserCredential? userCredential, Func<TService, Task<TResult>> evaluate) =>
        serviceEvaluator.EvaluateAsync(
            (DBSessionMode)mode,
            userCredential,
            evaluate,
            defaultCancellationTokenSource?.CancellationToken ?? CancellationToken.None);
}
