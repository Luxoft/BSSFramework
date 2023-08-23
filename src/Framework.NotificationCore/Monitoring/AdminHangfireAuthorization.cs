using Framework.DomainDriven;
using Framework.SecuritySystem;

using Hangfire.Dashboard;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.NotificationCore.Monitoring;

public class AdminHangfireAuthorization : IDashboardAuthorizationFilter
{
    private readonly IDBSessionEvaluator dbSessionEvaluator;

    public AdminHangfireAuthorization(IDBSessionEvaluator dbSessionEvaluator) => this.dbSessionEvaluator = dbSessionEvaluator;

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return false;
        }

        return this.dbSessionEvaluator.EvaluateAsync(
            DBSessionMode.Read,
            (sp, _) => Task.FromResult(sp.GetRequiredService<IAuthorizationSystem>().IsAdmin())).GetAwaiter().GetResult();
    }
}
