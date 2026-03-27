using Framework.Core;

namespace Framework.Database.Session;

public static class DbSessionEvaluatorExtensions
{
    public static Task EvaluateAsync(this IDBSessionEvaluator evaluator, DBSessionMode sessionMode, Func<IServiceProvider, Task> action) =>
        evaluator.EvaluateAsync(sessionMode, action.ToDefaultTask());
}
