using Framework.Authorization.BLL;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.NotificationCore.Monitoring;

using Hangfire.Dashboard;

namespace SampleSystem.WebApiCore;

/// <summary>
/// ВАЖНО: это пример, который подходит исключительно для Sample System с её захардкоженным текущим пользователем.
/// Для рабочих проектов рекоммендуется использовать AdminHangfireAuthorization (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
/// </summary>
public class SampleSystemHangfireAuthorization : IDashboardAuthorizationFilter
{
    private readonly IDBSessionEvaluator dbSessionEvaluator;

    private readonly IDashboardAuthorizationFilter baseFilter;

    public SampleSystemHangfireAuthorization(IDBSessionEvaluator dbSessionEvaluator)
    {
        this.baseFilter = new AdminHangfireAuthorization(dbSessionEvaluator);

        this.dbSessionEvaluator = dbSessionEvaluator;
    }

    public bool Authorize(DashboardContext context)
    {
        return this.dbSessionEvaluator.EvaluateAsync(
            DBSessionMode.Read,
            async (sp, _) =>
            {
                return this.baseFilter.Authorize(context)
                       || string.Compare(
                           sp.GetRequiredService<IAuthorizationBLLContext>().CurrentPrincipalName,
                           new DomainDefaultUserAuthenticationService().GetUserName(),
                           StringComparison.InvariantCultureIgnoreCase)
                       == 0;
            }).GetAwaiter().GetResult();
    }
}
