using Framework.Authorization.BLL;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.NotificationCore.Monitoring;
using Framework.SecuritySystem;

using Hangfire.Dashboard;

namespace SampleSystem.WebApiCore;

/// <summary>
/// ВАЖНО: это пример, который подходит исключительно для Sample System с её захардкоженным текущим пользователем.
/// Для рабочих проектов рекоммендуется использовать AdminHangfireAuthorization (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
/// </summary>
public class SampleSystemHangfireAuthorization : IDashboardAuthorizationFilter
{
    private readonly IServiceEvaluator<IAuthorizationSystem> authorizationSystemEvaluator;

    private readonly IDashboardAuthorizationFilter baseFilter;

    public SampleSystemHangfireAuthorization(IServiceEvaluator<IAuthorizationSystem> authorizationSystemEvaluator)
    {
        this.authorizationSystemEvaluator = authorizationSystemEvaluator;
        this.baseFilter = new AdminHangfireAuthorization(authorizationSystemEvaluator);
    }

    public bool Authorize(DashboardContext context)
    {
        return this.authorizationSystemEvaluator.Evaluate(
            DBSessionMode.Read,
            authSystem => this.baseFilter.Authorize(context)
                          || string.Compare(
                              authSystem.CurrentPrincipalName,
                              new DomainDefaultUserAuthenticationService().GetUserName(),
                              StringComparison.InvariantCultureIgnoreCase) == 0);
    }
}
