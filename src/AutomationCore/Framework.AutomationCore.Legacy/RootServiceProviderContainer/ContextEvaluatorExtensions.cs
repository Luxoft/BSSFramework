using Framework.BLL;
using Framework.Database;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public static class ContextEvaluatorExtensions
{
    public static TResult EvaluateWrite<TBLLContext, TResult>(this ISyncServiceEvaluator<TBLLContext> serviceEvaluator, Func<TBLLContext, TResult> func) => serviceEvaluator.Evaluate(DBSessionMode.Write, func);

    public static void EvaluateWrite<TBLLContext>(this ISyncServiceEvaluator<TBLLContext> serviceEvaluator, Action<TBLLContext> action) => serviceEvaluator.Evaluate(DBSessionMode.Write, action);

    public static void EvaluateRead<TBLLContext>(this ISyncServiceEvaluator<TBLLContext> serviceEvaluator, Action<TBLLContext> action) => serviceEvaluator.Evaluate(DBSessionMode.Read, action);

    public static TResult EvaluateRead<TBLLContext, TResult>(this ISyncServiceEvaluator<TBLLContext> serviceEvaluator, Func<TBLLContext, TResult> func) => serviceEvaluator.Evaluate(DBSessionMode.Read, func);
}

