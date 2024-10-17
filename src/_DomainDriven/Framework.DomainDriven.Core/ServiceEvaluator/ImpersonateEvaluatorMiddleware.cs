using Framework.Core.Services;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.ScopedEvaluate;
using Framework.SecuritySystem.Credential;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class ImpersonateEvaluatorMiddleware(IServiceProvider scopedServiceProvider, UserCredential? customUserCredential) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        var userAuthenticationService = scopedServiceProvider.GetRequiredService<IUserAuthenticationService>();
        var impersonateService = scopedServiceProvider.GetRequiredService<IImpersonateService>();

        var actualImpersonateService =
            customUserCredential != null && customUserCredential != userAuthenticationService.GetUserName()
                ? impersonateService
                : null;

        if (actualImpersonateService == null)
        {
            return await getResult();
        }
        else
        {
            return await actualImpersonateService.WithImpersonateAsync(customUserCredential, getResult);
        }
    }
}
