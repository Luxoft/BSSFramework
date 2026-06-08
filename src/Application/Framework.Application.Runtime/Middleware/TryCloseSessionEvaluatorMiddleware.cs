using Framework.Database;

namespace Framework.Application.Middleware;

public class TryCloseSessionEvaluatorMiddleware(IDBSessionManager dbSessionManager) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult, CancellationToken ct) =>
        await dbSessionManager.EvaluateAsync(getResult, ct);
}
