using Framework.Core;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.ServiceModel;

public static class ContextEvaluatorExtensions
{
    public static async Task<TResult> EvaluateAsync<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
    {
        return await contextEvaluator.EvaluateAsync(sessionMode, null, getResult);
    }

    public static async Task EvaluateAsync<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task> action)
    {
        await contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, evaluatedData => action(evaluatedData).ContinueWith(_ => default(object)));
    }

    public static async Task EvaluateAsync<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task> action)
    {
        await contextEvaluator.EvaluateAsync(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, Action<EvaluatedData<TBLLContext, TDTOMappingService>> action)
    {
        contextEvaluator.Evaluate(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Action<EvaluatedData<TBLLContext, TDTOMappingService>> action)
    {
        contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
    }

    public static TResult Evaluate<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, null, getResult);
    }

    public static TResult Evaluate<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, c => Task.FromResult(getResult(c))).GetAwaiter().GetResult();
    }
}
