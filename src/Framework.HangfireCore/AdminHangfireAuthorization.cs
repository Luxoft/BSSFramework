using Framework.SecuritySystem;

using Hangfire.Dashboard;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.HangfireCore;

public class AdminHangfireAuthorization(DomainSecurityRule.RoleBaseSecurityRule securityRule) : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        return httpContext.User.Identity is { IsAuthenticated: true }
               && httpContext.RequestServices
                             .GetRequiredKeyedService<ISecuritySystem>(nameof(SecurityRuleCredential.CurrentUserWithoutRunAsCredential))
                             .HasAccess(securityRule);
    }
}
