using Framework.Core;
using Framework.DomainDriven;

namespace Framework.HangfireCore.JobServices;

public static class ServiceJobEvaluatorExtensions
{
    public static async Task ExecuteAsync<TService>(this IServiceJobEvaluator serviceJobEvaluator, DBSessionMode sessionMode, Func<TService, Task> evaluate)
    {
        await serviceJobEvaluator.ExecuteAsync<TService, object>(
            sessionMode,
            async service =>
            {
                await evaluate(service);

                return default;
            });
    }

    public static async Task Execute<TService>(this IServiceJobEvaluator serviceJobEvaluator, DBSessionMode sessionMode, Action<TService> evaluate)
    {
        await serviceJobEvaluator.Execute(sessionMode, evaluate.ToDefaultFunc());
    }
    public static async Task<TResult> Execute<TService, TResult>(this IServiceJobEvaluator serviceJobEvaluator, DBSessionMode sessionMode, Func<TService, TResult> evaluate)
    {
        return await serviceJobEvaluator.ExecuteAsync<TService, TResult>(
                   sessionMode,
                   async service => evaluate(service));
    }
}
