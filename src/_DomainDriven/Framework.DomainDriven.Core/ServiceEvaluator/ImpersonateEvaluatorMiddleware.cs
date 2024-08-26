using Framework.Core.Services;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class ImpersonateEvaluatorMiddleware : IScopedEvaluatorMiddleware
{
    private readonly IServiceProvider scopedServiceProvider;

    private readonly string customPrincipalName;

    public ImpersonateEvaluatorMiddleware(IServiceProvider scopedServiceProvider, string customPrincipalName)
    {
        this.scopedServiceProvider = scopedServiceProvider;
        this.customPrincipalName = customPrincipalName;
    }

    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        var userAuthenticationService = this.scopedServiceProvider.GetRequiredService<IUserAuthenticationService>();
        var impersonateService = this.scopedServiceProvider.GetRequiredService<IImpersonateService>();

        var actualImpersonateService =
            this.customPrincipalName != null && this.customPrincipalName != userAuthenticationService.GetUserName()
                ? impersonateService
                : null;

        if (actualImpersonateService == null)
        {
            return await getResult();
        }
        else
        {
            return await actualImpersonateService.WithImpersonateAsync(this.customPrincipalName, getResult);
        }
    }
}
