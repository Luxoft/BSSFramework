using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.NotificationCore.Monitoring;

using Hangfire.Dashboard;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.ServiceEnvironment;
using WorkflowSampleSystem.WebApiCore.Env;

namespace WorkflowSampleSystem.WebApiCore
{
    /// <summary>
    /// ВАЖНО: это пример, который подходит исключительно для Sample System с её захардкоженным текущим пользователем.
    /// Для рабочих проектов рекоммендуется использовать AdminHangfireAuthorization (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
    /// </summary>
    public class WorkflowSampleSystemHangfireAuthorization : IDashboardAuthorizationFilter
    {
        private readonly Lazy<WorkflowSampleSystemServiceEnvironment> environment;

        private readonly IDashboardAuthorizationFilter baseFilter;

        public WorkflowSampleSystemHangfireAuthorization(Lazy<WorkflowSampleSystemServiceEnvironment> environment)
        {
            this.baseFilter = new AdminHangfireAuthorization<IWorkflowSampleSystemBLLContext>(LazyHelper.Create(() => (IServiceEnvironment<IWorkflowSampleSystemBLLContext>)this.environment.Value));

            this.environment = environment;
        }

        public bool Authorize(DashboardContext context)
        {
            return this.environment.Value.GetContextEvaluator().Evaluate(
                DBSessionMode.Read,
                z =>
                {
                    return this.baseFilter.Authorize(context)
                        || string.Compare(z.Authorization.CurrentPrincipalName, UserAuthenticationService.CurrentUser, StringComparison.InvariantCultureIgnoreCase) == 0;
                });
        }
    }
}
