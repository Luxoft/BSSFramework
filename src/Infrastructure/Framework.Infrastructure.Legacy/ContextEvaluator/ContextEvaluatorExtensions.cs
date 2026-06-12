using Anch.Core;
using Anch.SecuritySystem;

using Framework.Database;
using Framework.Infrastructure.Services;

namespace Framework.Infrastructure.ContextEvaluator;

public static class ContextEvaluatorExtensions
{
    extension<TBLLContext, TMappingService>(IContextEvaluator<TBLLContext, TMappingService> contextEvaluator)
    {
        public async Task<TResult> EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult,
            CancellationToken ct) =>
            await contextEvaluator.EvaluateAsync<TResult>(sessionMode, null, getResult, ct);

        public async Task EvaluateAsync(
            DBSessionMode sessionMode,
            UserCredential? customUserCredential,
            Func<EvaluatedData<TBLLContext, TMappingService>, Task> action,
            CancellationToken ct) =>
            await contextEvaluator.EvaluateAsync(sessionMode, customUserCredential, action.ToDefaultTask(), ct);

        public async Task EvaluateAsync(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, Task> action, CancellationToken ct) =>
            await contextEvaluator.EvaluateAsync(sessionMode, null, action.ToDefaultTask(), ct);

        public void Evaluate(DBSessionMode sessionMode, Action<EvaluatedData<TBLLContext, TMappingService>> action) =>
            contextEvaluator.Evaluate(sessionMode, null, action);

        public void Evaluate(DBSessionMode sessionMode, UserCredential? customUserCredential, Action<EvaluatedData<TBLLContext, TMappingService>> action) =>
            contextEvaluator.Evaluate(sessionMode, customUserCredential, action.ToDefaultFunc());

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult) =>
            contextEvaluator.Evaluate(sessionMode, null, getResult);

        public TResult Evaluate<TResult>(
            DBSessionMode sessionMode,
            UserCredential? customUserCredential,
            Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult) =>
            contextEvaluator.EvaluateAsync<TResult>(sessionMode, customUserCredential, async c => getResult(c), CancellationToken.None).GetAwaiter()
                            .GetResult();
    }
}
