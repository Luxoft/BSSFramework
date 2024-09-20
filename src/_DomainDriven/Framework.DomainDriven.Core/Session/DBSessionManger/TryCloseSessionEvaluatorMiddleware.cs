using Framework.DomainDriven.ScopedEvaluate;

namespace Framework.DomainDriven;

public class TryCloseSessionEvaluatorMiddleware(IDBSessionManager dbSessionManager) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        return await dbSessionManager.EvaluateAsync(getResult);
    }
}
