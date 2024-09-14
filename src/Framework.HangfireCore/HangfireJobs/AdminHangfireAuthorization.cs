using Framework.SecuritySystem;

using Hangfire.Dashboard;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.HangfireCore;

public class AdminHangfireAuthorization(DomainSecurityRule.RoleBaseSecurityRule securityRule) : IDashboardAuthorizationFilter
{
    public AdminHangfireAuthorization()
        : this(SecurityRole.Administrator)
    {
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        return httpContext.User.Identity is { IsAuthenticated: true }
               && httpContext.RequestServices.GetRequiredService<ISecuritySystemFactory>().Create(false).HasAccess(securityRule);
    }
}
