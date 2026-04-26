using Anch.Core;

namespace Framework.Application.Jobs;

public static class JobServiceEvaluatorExtensions
{
    public static async Task EvaluateAsync<TService>(this IJobServiceEvaluator<TService> jobServiceEvaluator, Func<TService, Task> executeAsync)
        where TService : notnull =>
        await jobServiceEvaluator.EvaluateAsync(executeAsync.ToDefaultTask());
}
