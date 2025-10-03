﻿using Framework.DomainDriven.Auth;
using Framework.DomainDriven.ScopedEvaluate;
using SecuritySystem.Credential;
using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Services;

namespace Framework.DomainDriven;

public class ImpersonateEvaluatorMiddleware(IServiceProvider scopedServiceProvider, UserCredential? customUserCredential) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        var userAuthenticationService = scopedServiceProvider.GetRequiredService<IRawUserAuthenticationService>();
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
