using Framework.DomainDriven;

namespace Automation.ServiceEnvironment;

public static class ContextEvaluatorExtensions
{
    public static TResult EvaluateWrite<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, Func<TBLLContext, TResult> func)
    {
        return contextEvaluator.Evaluate(DBSessionMode.Write, func);
    }

    public static void EvaluateWrite<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, Action<TBLLContext> action)
    {
        contextEvaluator.Evaluate(DBSessionMode.Write, action);
    }

    public static void EvaluateRead<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, Action<TBLLContext> action)
    {
        contextEvaluator.Evaluate(DBSessionMode.Read, action);
    }

    public static TResult EvaluateRead<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, Func<TBLLContext, TResult> func)
    {
        return contextEvaluator.Evaluate(DBSessionMode.Read, func);
    }
}
