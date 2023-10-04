using Framework.Core;

namespace Framework.DomainDriven;

public static class ContextEvaluatorExtensions
{
    public static Task<TResult> EvaluateAsync<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Func<TBLLContext, IDBSession, Task<TResult>> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, null, getResult);
    }

    public static Task<TResult> EvaluateAsync<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Func<TBLLContext, Task<TResult>> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, null, (ctx, _) => getResult(ctx));
    }

    public static Task EvaluateAsync<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, IDBSession, Task> action)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, async (ctx, session) =>
                                                                                {
                                                                                    await action(ctx, session);
                                                                                    return default(object);
                                                                                });
    }

    public static Task EvaluateAsync<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Func<TBLLContext, IDBSession, Task> action)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, null, action);
    }

    public static Task EvaluateAsync<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Func<TBLLContext, Task> action)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, null, (ctx, _) => action(ctx));
    }

    public static void Evaluate<TBLLContext>(
            this IContextEvaluator<TBLLContext> contextEvaluator,
            DBSessionMode sessionMode,
            Action<TBLLContext, IDBSession> action)
    {
        contextEvaluator.Evaluate(sessionMode, action.ToDefaultFunc());
    }

    public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Action<TBLLContext, IDBSession> action)
    {
        contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
    }

    public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Func<TBLLContext, IDBSession, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, null, getResult);
    }

    public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Action<TBLLContext> action)
    {
        contextEvaluator.Evaluate(sessionMode, null, action);
    }

    public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Action<TBLLContext> action)
    {
        contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
    }

    public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, Func<TBLLContext, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, null, getResult);
    }

    public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, customPrincipalName, (c, _) => getResult(c));
    }

    public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, IDBSession, TResult> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, (c, s) => Task.FromResult(getResult(c, s))).GetAwaiter().GetResult();
    }
}
