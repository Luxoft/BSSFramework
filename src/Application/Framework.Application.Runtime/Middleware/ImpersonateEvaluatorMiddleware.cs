using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.Services;

namespace Framework.Application.Middleware;

public class ImpersonateEvaluatorMiddleware(IServiceProvider scopedServiceProvider, UserCredential customUserCredential) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        var impersonateService = scopedServiceProvider.GetRequiredService<IImpersonateService>();

        return await impersonateService.WithImpersonateAsync(customUserCredential, getResult);
    }
}
