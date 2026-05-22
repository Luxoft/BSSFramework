using Framework.Database;
using Framework.Infrastructure.ContextEvaluator;

using Anch.SecuritySystem;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public static class ContextEvaluatorWithMappingExtensions
{
    public static TResult EvaluateRead<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, UserCredential? userCredential, Func<TBLLContext, TMappingService, TResult> func) =>
        contextEvaluator.Evaluate(
            DBSessionMode.Read,
            userCredential,
            evaluateData => func(evaluateData.Context, evaluateData.MappingService));

    public static TResult EvaluateWrite<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, UserCredential? userCredential, Func<TBLLContext, TMappingService, TResult> func) =>
        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            userCredential,
            evaluateData => func(evaluateData.Context, evaluateData.MappingService));

    public static void EvaluateWrite<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, UserCredential? userCredential, Action<TBLLContext, TMappingService> action) =>
        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            userCredential,
            evaluateData => action(evaluateData.Context, evaluateData.MappingService));

    public static TResult EvaluateRead<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, Func<TBLLContext, TMappingService, TResult> func) => contextEvaluator.EvaluateRead(null, func);

    public static TResult EvaluateWrite<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, Func<TBLLContext, TMappingService, TResult> func) => contextEvaluator.EvaluateWrite(null, func);

    public static void EvaluateWrite<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, Action<TBLLContext, TMappingService> action) => contextEvaluator.EvaluateWrite(null, action);
}
