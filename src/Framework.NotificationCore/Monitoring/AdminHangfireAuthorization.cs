using System;

using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;

using Hangfire.Dashboard;

namespace Framework.NotificationCore.Monitoring;

public class AdminHangfireAuthorization<TBllContext> : IDashboardAuthorizationFilter
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
{
    private readonly Lazy<IServiceEnvironment<TBllContext>> environment;

    public AdminHangfireAuthorization(Lazy<IServiceEnvironment<TBllContext>> environment) => this.environment = environment;

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return false;
        }

        return this.environment.Value
                   .GetContextEvaluator()
                   .Evaluate(
                             DBSessionMode.Read,
                             z => z.Authorization.Logics.BusinessRole.HasAdminRole());
    }
}
