using Framework.Application.Session;

namespace Framework.Application.Middleware;

public class TryCloseSessionEvaluatorMiddleware(IDBSessionManager dbSessionManager) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        return await dbSessionManager.EvaluateAsync(getResult);
    }
}
