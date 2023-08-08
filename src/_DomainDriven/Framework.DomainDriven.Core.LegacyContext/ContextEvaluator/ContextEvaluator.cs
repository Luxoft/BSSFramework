using Framework.Core.Services;
using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class ContextEvaluator<TBLLContext> : IContextEvaluator<TBLLContext>
{
    private readonly IDBSessionEvaluator dbSessionEvaluator;

    public ContextEvaluator([NotNull] IDBSessionEvaluator dbSessionEvaluator)
    {
        this.dbSessionEvaluator = dbSessionEvaluator ?? throw new ArgumentNullException(nameof(dbSessionEvaluator));
    }

    public Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, IDBSession, Task<TResult>> getResult)
    {
        return this.dbSessionEvaluator.EvaluateAsync(sessionMode, async (scopeServiceProvider, session) =>
        {
            var defaultPrincipalName = scopeServiceProvider.GetRequiredService<IUserAuthenticationService>().GetUserName();

            var impersonateService = !string.IsNullOrWhiteSpace(customPrincipalName) && customPrincipalName != defaultPrincipalName
                    ? scopeServiceProvider.GetRequiredService<IImpersonateService>()
                    : null;

            var context = scopeServiceProvider.GetRequiredService<TBLLContext>();

            if (impersonateService == null)
            {
                return await getResult(context, session);
            }
            else
            {
                return await impersonateService.WithImpersonateAsync(customPrincipalName, () => getResult(context, session));
            }
        });
    }
}
