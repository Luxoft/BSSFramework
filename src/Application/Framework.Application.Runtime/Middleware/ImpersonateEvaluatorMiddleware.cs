using Anch.SecuritySystem;
using Anch.SecuritySystem.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application.Middleware;

public class ImpersonateEvaluatorMiddleware(IServiceProvider scopedServiceProvider, UserCredential customUserCredential) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult, CancellationToken ct)
    {
        var impersonateService = scopedServiceProvider.GetRequiredService<IImpersonateService>();

        return await impersonateService.WithImpersonateAsync(customUserCredential, getResult);
    }
}

