using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.NotificationCore.Monitoring;

using Hangfire.Dashboard;

using SampleSystem.BLL;

namespace SampleSystem.WebApiCore;

/// <summary>
/// ВАЖНО: это пример, который подходит исключительно для Sample System с её захардкоженным текущим пользователем.
/// Для рабочих проектов рекоммендуется использовать AdminHangfireAuthorization (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
/// </summary>
public class SampleSystemHangfireAuthorization : IDashboardAuthorizationFilter
{
    private readonly IContextEvaluator<ISampleSystemBLLContext> contextEvaluator;

    private readonly IDashboardAuthorizationFilter baseFilter;

    public SampleSystemHangfireAuthorization(IContextEvaluator<ISampleSystemBLLContext> contextEvaluator)
    {
        this.baseFilter = new AdminHangfireAuthorization<ISampleSystemBLLContext>(contextEvaluator);

        this.contextEvaluator = contextEvaluator;
    }

    public bool Authorize(DashboardContext context)
    {
        return this.contextEvaluator.Evaluate(
                                              DBSessionMode.Read,
                                              z =>
                                              {
                                                  return this.baseFilter.Authorize(context)
                                                         || string.Compare(z.Authorization.CurrentPrincipalName, new DomainDefaultUserAuthenticationService().GetUserName(), StringComparison.InvariantCultureIgnoreCase) == 0;
                                              });
    }
}
