using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel;
using SecuritySystem.Credential;

namespace Automation.ServiceEnvironment;

public static class ContextEvaluatorWithMappingExtensions
{
    public static TResult EvaluateRead<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, UserCredential? userCredential, Func<TBLLContext, TMappingService, TResult> func)
    {
        return contextEvaluator.Evaluate(
            DBSessionMode.Read,
            userCredential,
            evaluateData => func(evaluateData.Context, evaluateData.MappingService));
    }

    public static TResult EvaluateWrite<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, UserCredential? userCredential, Func<TBLLContext, TMappingService, TResult> func)
    {
        return contextEvaluator.Evaluate(
            DBSessionMode.Write,
            userCredential,
            evaluateData => func(evaluateData.Context, evaluateData.MappingService));
    }

    public static void EvaluateWrite<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, UserCredential? userCredential, Action<TBLLContext, TMappingService> action)
    {
        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            userCredential,
            evaluateData => action(evaluateData.Context, evaluateData.MappingService));
    }

    public static TResult EvaluateRead<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, Func<TBLLContext, TMappingService, TResult> func)
    {
        return contextEvaluator.EvaluateRead(null, func);
    }

    public static TResult EvaluateWrite<TBLLContext, TMappingService, TResult>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, Func<TBLLContext, TMappingService, TResult> func)
    {
        return contextEvaluator.EvaluateWrite(null, func);
    }

    public static void EvaluateWrite<TBLLContext, TMappingService>(this IContextEvaluator<TBLLContext, TMappingService> contextEvaluator, Action<TBLLContext, TMappingService> action)
    {
        contextEvaluator.EvaluateWrite(null, action);
    }
}
