using Framework.DomainDriven.WebApiNetCore.Auth;
using Framework.HangfireCore;
using Framework.SecuritySystem.UserSource;

using Hangfire.Dashboard;

namespace SampleSystem.WebApiCore;

/// <summary>
/// ВАЖНО: это пример, который подходит исключительно для Sample System с её захардкоженным текущим пользователем.
/// Для рабочих проектов рекоммендуется использовать AdminHangfireAuthorization (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
/// </summary>
public class SampleSystemHangfireAuthorization : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        return new AdminHangfireAuthorization().Authorize(context)
               || new EnvironmentDefaultUserAuthenticationServiceSettings().DefaultValue
               == httpContext.RequestServices.GetRequiredService<ICurrentUser>().Name;
    }
}
