using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel;

namespace Automation.ServiceEnvironment;

public static class ContextEvaluatorWithMappingExtensions
{
    public static TResult EvaluateRead<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, string principalName, Func<TBLLContext, TDTOMappingService, TResult> func)
    {
        return contextEvaluator.Evaluate(
            DBSessionMode.Read,
            principalName,
            evaluateData => func(evaluateData.Context, evaluateData.MappingService));
    }

    public static TResult EvaluateWrite<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, string principalName, Func<TBLLContext, TDTOMappingService, TResult> func)
    {
        return contextEvaluator.Evaluate(
            DBSessionMode.Write,
            principalName,
            evaluateData => func(evaluateData.Context, evaluateData.MappingService));
    }

    public static void EvaluateWrite<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, string principalName, Action<TBLLContext, TDTOMappingService> action)
    {
        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            principalName,
            evaluateData => action(evaluateData.Context, evaluateData.MappingService));
    }

    public static TResult EvaluateRead<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, Func<TBLLContext, TDTOMappingService, TResult> func)
    {
        return contextEvaluator.EvaluateRead(null, func);
    }

    public static TResult EvaluateWrite<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, Func<TBLLContext, TDTOMappingService, TResult> func)
    {
        return contextEvaluator.EvaluateWrite(null, func);
    }

    public static void EvaluateWrite<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, Action<TBLLContext, TDTOMappingService> action)
    {
        contextEvaluator.EvaluateWrite(null, action);
    }
}
