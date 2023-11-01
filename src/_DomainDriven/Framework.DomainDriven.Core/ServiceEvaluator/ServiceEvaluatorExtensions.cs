using Framework.Core;

namespace Framework.DomainDriven;

public static class ServiceEvaluatorExtensions
{
    public static async Task<TResult> EvaluateAsync<TService, TResult>(this IServiceEvaluator<TService> contextEvaluator, DBSessionMode sessionMode, Func<TService, Task<TResult>> getResult)
    {
        return await contextEvaluator.EvaluateAsync(sessionMode, null, getResult);
    }

    public static async Task EvaluateAsync<TService>(this IServiceEvaluator<TService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<TService, Task> action)
    {
        await contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, async service =>
                                                                                {
                                                                                    await action(service);
                                                                                    return default(object);
                                                                                });
    }

    public static async Task EvaluateAsync<TService>(this IServiceEvaluator<TService> contextEvaluator, DBSessionMode sessionMode, Func<TService, Task> action)
    {
        await contextEvaluator.EvaluateAsync(sessionMode, null, action);
    }

    public static void Evaluate<TService>(
            this IServiceEvaluator<TService> contextEvaluator,
            DBSessionMode sessionMode,
            Action<TService> action)
    {
        contextEvaluator.Evaluate(sessionMode, action.ToDefaultFunc());
    }

    public static void Evaluate<TService>(this IServiceEvaluator<TService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Action<TService> action)
    {
        contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
    }

    public static TResult Evaluate<TService, TResult>(this IServiceEvaluator<TService> contextEvaluator, DBSessionMode sessionMode, Func<TService, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, null, getResult);
    }

    public static TResult Evaluate<TService, TResult>(this IServiceEvaluator<TService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, Func<TService, TResult> getResult)
    {
        return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, service => Task.FromResult(getResult(service))).GetAwaiter().GetResult();
    }
}
