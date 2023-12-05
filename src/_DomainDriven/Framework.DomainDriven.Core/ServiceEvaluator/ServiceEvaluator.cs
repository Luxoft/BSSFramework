using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class ServiceEvaluator<TService> : IServiceEvaluator<TService>
{
    private readonly IServiceProvider rootServiceProvider;

    public ServiceEvaluator(IServiceProvider rootServiceProvider)
    {
        this.rootServiceProvider = rootServiceProvider;
    }

    public async Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        string customPrincipalName,
        Func<TService, Task<TResult>> getResult)
    {
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var sessionMiddleware = new SessionEvaluatorMiddleware(scope.ServiceProvider, sessionMode);

        var impersonateMiddleware = new ImpersonateEvaluatorMiddleware(scope.ServiceProvider, customPrincipalName);

        return await sessionMiddleware.With(impersonateMiddleware).EvaluateAsync(
                   async () => await getResult(scope.ServiceProvider.GetRequiredService<TService>()));
    }
}
