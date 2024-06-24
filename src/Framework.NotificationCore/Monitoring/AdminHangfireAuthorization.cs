using Framework.DomainDriven;
using Framework.SecuritySystem;

using Hangfire.Dashboard;

namespace Framework.NotificationCore.Monitoring;

public class AdminHangfireAuthorization : IDashboardAuthorizationFilter
{
    private readonly IServiceEvaluator<IAuthorizationSystem> authorizationSystemEvaluator;

    public AdminHangfireAuthorization(IServiceEvaluator<IAuthorizationSystem> authorizationSystemEvaluator) => this.authorizationSystemEvaluator = authorizationSystemEvaluator;

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return false;
        }

        return this.authorizationSystemEvaluator.Evaluate(DBSessionMode.Read, service => service.IsAdministrator());
    }
}
