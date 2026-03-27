using Framework.Application.Middleware;
using Framework.Application.Session;
using Framework.Application.Session.DBSession;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Credential;

namespace Framework.Application.ServiceEvaluator;

public class ServiceEvaluator<TService>(IServiceProvider rootServiceProvider) : IServiceEvaluator<TService>
    where TService : notnull
{
    public async Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? userCredential,
        Func<TService, Task<TResult>> getResult)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var sessionMiddleware = new SessionEvaluatorMiddleware(scope.ServiceProvider, sessionMode);

        var impersonateMiddleware = new ImpersonateEvaluatorMiddleware(scope.ServiceProvider, userCredential);

        return await sessionMiddleware.With(impersonateMiddleware).EvaluateAsync(
                   async () => await getResult(scope.ServiceProvider.GetRequiredService<TService>()));
    }
}
