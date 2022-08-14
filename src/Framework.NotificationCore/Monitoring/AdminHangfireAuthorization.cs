using System;

using Framework.Authorization.BLL;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Hangfire.Dashboard;

namespace Framework.NotificationCore.Monitoring;

public class AdminHangfireAuthorization<TBllContext> : IDashboardAuthorizationFilter
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
{
    private readonly IContextEvaluator<TBllContext> contextEvaluator;

    public AdminHangfireAuthorization(IContextEvaluator<TBllContext> contextEvaluator) => this.contextEvaluator = contextEvaluator;

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return false;
        }

        return this.contextEvaluator.Evaluate(DBSessionMode.Read, z => z.Authorization.Logics.BusinessRole.HasAdminRole());
    }
}
