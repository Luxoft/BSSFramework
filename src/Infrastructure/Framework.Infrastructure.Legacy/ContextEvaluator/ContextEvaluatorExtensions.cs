using CommonFramework;

using Framework.Database;
using Framework.Infrastructure.Service;

using SecuritySystem;

namespace Framework.Infrastructure.ContextEvaluator;

public static class ContextEvaluatorExtensions
{
    extension<TBLLContext, TMappingService>(IContextEvaluator<TBLLContext, TMappingService> contextEvaluator)
    {
        public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult) =>
            await contextEvaluator.EvaluateAsync<TResult>(sessionMode, null, getResult);

        public async Task EvaluateAsync(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, Task> action) =>
            await contextEvaluator.EvaluateAsync(sessionMode, customUserCredential, action.ToDefaultTask());

        public async Task EvaluateAsync(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, Task> action) =>
            await contextEvaluator.EvaluateAsync(sessionMode, null, action);

        public void Evaluate(DBSessionMode sessionMode, Action<EvaluatedData<TBLLContext, TMappingService>> action) => contextEvaluator.Evaluate(sessionMode, null, action);

        public void Evaluate(DBSessionMode sessionMode, UserCredential? customUserCredential, Action<EvaluatedData<TBLLContext, TMappingService>> action) =>
            contextEvaluator.Evaluate(sessionMode, customUserCredential, action.ToDefaultFunc());

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult) =>
            contextEvaluator.Evaluate(sessionMode, null, getResult);

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult) =>
            contextEvaluator.EvaluateAsync<TResult>(sessionMode, customUserCredential, async c => getResult(c)).GetAwaiter().GetResult();
    }
}
