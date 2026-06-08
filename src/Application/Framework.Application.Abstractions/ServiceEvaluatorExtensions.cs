using Anch.Core;
using Anch.SecuritySystem;

using Framework.Database;

namespace Framework.Application;

public static class ServiceEvaluatorExtensions
{
    extension<TService>(IServiceEvaluator<TService> serviceEvaluator)
    {
        public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> getResult, CancellationToken ct) =>
            await serviceEvaluator.EvaluateAsync(sessionMode, null, getResult, ct);

        public async Task EvaluateAsync(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, Task> action, CancellationToken ct) =>
            await serviceEvaluator.EvaluateAsync(sessionMode, customUserCredential, action.ToDefaultTask(), ct);

        public async Task EvaluateAsync(DBSessionMode sessionMode, Func<TService, Task> action, CancellationToken ct) =>
            await serviceEvaluator.EvaluateAsync(sessionMode, null, action, ct);
    }
}
