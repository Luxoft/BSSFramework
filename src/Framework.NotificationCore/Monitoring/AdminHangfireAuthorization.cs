using Framework.SecuritySystem;

using Hangfire.Dashboard;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.NotificationCore.Monitoring;

public class AdminHangfireAuthorization : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        return httpContext.User.Identity is { IsAuthenticated: true }
               && httpContext.RequestServices.GetRequiredService<ISecuritySystem>().IsAdministrator();
    }
}
