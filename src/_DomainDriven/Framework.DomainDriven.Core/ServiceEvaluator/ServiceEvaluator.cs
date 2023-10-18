using Framework.Core.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class ServiceEvaluator<TService> : IServiceEvaluator<TService>
{
    private readonly IDBSessionEvaluator dbSessionEvaluator;

    public ServiceEvaluator(IDBSessionEvaluator dbSessionEvaluator)
    {
        this.dbSessionEvaluator = dbSessionEvaluator ?? throw new ArgumentNullException(nameof(dbSessionEvaluator));
    }

    public Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        string customPrincipalName,
        Func<TService, Task<TResult>> getResult)
    {
        return this.dbSessionEvaluator.EvaluateAsync(
            sessionMode,
            async serviceProvider =>
            {
                var scopeEvaluator = ActivatorUtilities.CreateInstance<ScopeEvaluator<TService>>(serviceProvider, customPrincipalName);

                return await scopeEvaluator.Invoke(getResult);
            });
    }

    private class ScopeEvaluator<TService>
    {
        private readonly TService service;

        private readonly string customPrincipalName;

        private readonly IImpersonateService actualImpersonateService;

        public ScopeEvaluator(
            TService service,
            IUserAuthenticationService userAuthenticationService,
            IImpersonateService impersonateService,
            string customPrincipalName)
        {
            this.service = service;
            this.customPrincipalName = customPrincipalName;
            this.actualImpersonateService = !string.IsNullOrWhiteSpace(customPrincipalName)
                                            && customPrincipalName != userAuthenticationService.GetUserName()
                                                ? impersonateService
                                                : null;
        }

        public async Task<TResult> Invoke<TResult>(Func<TService, Task<TResult>> getResult)
        {
            if (this.actualImpersonateService == null)
            {
                return await getResult(this.service);
            }
            else
            {
                return await this.actualImpersonateService.WithImpersonateAsync(this.customPrincipalName, () => getResult(this.service));
            }
        }
    }
}
