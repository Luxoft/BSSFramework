using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class DBSessionEvaluator(IServiceProvider rootServiceProvider) : IDBSessionEvaluator
{
    public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, Task<TResult>> getResult)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var sessionMiddleware = new SessionEvaluatorMiddleware(scope.ServiceProvider, sessionMode);

        return await sessionMiddleware.EvaluateAsync(async () => await getResult(scope.ServiceProvider));
    }
}
