using Framework.Core;

namespace Framework.DomainDriven;

public static class DBSessionEvaluatorExtensions
{
    public static Task EvaluateAsync(this IDBSessionEvaluator evaluator, DBSessionMode sessionMode, Func<IServiceProvider, Task> action) =>
        evaluator.EvaluateAsync(sessionMode, action.ToDefaultTask());
}
