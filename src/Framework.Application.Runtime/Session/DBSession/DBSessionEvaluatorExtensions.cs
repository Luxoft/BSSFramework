using Framework.Core;

namespace Framework.Application.Session.DBSession;

public static class DBSessionEvaluatorExtensions
{
    public static Task EvaluateAsync(this IDBSessionEvaluator evaluator, DBSessionMode sessionMode, Func<IServiceProvider, Task> action) =>
        evaluator.EvaluateAsync(sessionMode, action.ToDefaultTask());
}
