using Framework.Core;
using Framework.DomainDriven.ServiceModel.Service;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel;

public static class ContextEvaluatorExtensions
{
    public static Task<TResult> EvaluateAsync<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, null, getResult);
    }

    public static Task EvaluateAsync<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task> action)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, evaluatedData => action(evaluatedData).ContinueWith(_ => default(object)));
    }

    public static Task EvaluateAsync<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task> action)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Action<EvaluatedData<TBLLContext, TDTOMappingService>> action)
    {
        contextEvaluator.Evaluate(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Action<EvaluatedData<TBLLContext, TDTOMappingService>> action)
    {
        contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
    }

    public static TResult Evaluate<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, null, getResult);
    }

    public static TResult Evaluate<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, c => Task.FromResult(getResult(c))).GetAwaiter().GetResult();
    }
}
