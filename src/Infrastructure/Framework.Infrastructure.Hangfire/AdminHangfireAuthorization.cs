using Hangfire.Dashboard;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;

namespace Framework.Infrastructure.Hangfire;

public class AdminHangfireAuthorization(DomainSecurityRule.RoleBaseSecurityRule securityRule) : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        return httpContext.User.Identity is { IsAuthenticated: true }
               && httpContext.RequestServices
                             .GetRequiredKeyedService<ISecuritySystem>(nameof(SecurityRuleCredential.CurrentUserWithoutRunAsCredential))
                             .HasAccessAsync(securityRule, httpContext.RequestAborted)
                             .GetAwaiter()
                             .GetResult();
    }
}
