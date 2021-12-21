using Hangfire.Dashboard;

namespace Framework.HangfireCore
{
    public class BaseHangfireAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User?.Identity?.IsAuthenticated == true;
        }
    }
}
