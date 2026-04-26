using Framework.Application.Middleware;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;

namespace Framework.Application;

public class ServiceEvaluator<TService>(IServiceProvider rootServiceProvider) : IServiceEvaluator<TService>
    where TService : notnull
{
    public async Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? userCredential,
        Func<TService, Task<TResult>> getResult)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        return await GetMiddlewares(scope.ServiceProvider, sessionMode, userCredential)
                     .Aggregate()
                     .EvaluateAsync(async () => await getResult(scope.ServiceProvider.GetRequiredService<TService>()));
    }

    private static IEnumerable<IScopedEvaluatorMiddleware> GetMiddlewares(IServiceProvider serviceProvider, DBSessionMode sessionMode, UserCredential? userCredential)
    {
        yield return new SessionEvaluatorMiddleware(serviceProvider, sessionMode);

        if (userCredential != null)
        {
            yield return new ImpersonateEvaluatorMiddleware(serviceProvider, userCredential);
        }
    }
}
