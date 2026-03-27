using Framework.BLL.ServiceModel.Service;
using Framework.Core;
using Framework.Database;

using SecuritySystem.Credential;

namespace Framework.BLL.ServiceModel.ContextEvaluator;

public static class ContextEvaluatorExtensions
{
    public static async Task<TResult> EvaluateAsync<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult)
    {
        return await contextEvaluator.EvaluateAsync<TResult>(sessionMode, null, getResult);
    }

    public static async Task EvaluateAsync<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, Task> action)
    {
        await contextEvaluator.EvaluateAsync<object>(sessionMode, customUserCredential, async evaluatedData =>
        {
            await action(evaluatedData);
            return default(object);
        });
    }

    public static async Task EvaluateAsync<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, Task> action)
    {
        await contextEvaluator.EvaluateAsync(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, Action<EvaluatedData<TBLLContext, TMappingService>> action)
    {
        contextEvaluator.Evaluate(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, UserCredential? customUserCredential, Action<EvaluatedData<TBLLContext, TMappingService>> action)
    {
        contextEvaluator.Evaluate(sessionMode, customUserCredential, action.ToDefaultFunc());
    }

    public static TResult Evaluate<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, null, getResult);
    }

    public static TResult Evaluate<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        return contextEvaluator.EvaluateAsync<TResult>(sessionMode, customUserCredential, async c => getResult(c)).GetAwaiter().GetResult();
    }
}
