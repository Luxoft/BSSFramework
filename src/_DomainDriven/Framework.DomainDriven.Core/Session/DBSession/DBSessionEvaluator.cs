using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class DBSessionEvaluator : IDBSessionEvaluator
{
    private readonly IServiceProvider rootServiceProvider;

    public DBSessionEvaluator(IServiceProvider rootServiceProvider)
    {
        this.rootServiceProvider = rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
    }

    public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, Task<TResult>> getResult)
    {
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var sessionMiddleware = new SessionEvaluatorMiddleware(scope.ServiceProvider, sessionMode);

        return await sessionMiddleware.EvaluateAsync(async () => await getResult(scope.ServiceProvider));
    }
}
