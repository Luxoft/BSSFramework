using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.NotificationCore.Monitoring;

using Hangfire.Dashboard;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.ServiceEnvironment;
using AttachmentsSampleSystem.WebApiCore.Env;

namespace AttachmentsSampleSystem.WebApiCore
{
    /// <summary>
    /// ВАЖНО: это пример, который подходит исключительно для Sample System с её захардкоженным текущим пользователем.
    /// Для рабочих проектов рекоммендуется использовать AdminHangfireAuthorization (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
    /// </summary>
    public class AttachmentsSampleSystemHangfireAuthorization : IDashboardAuthorizationFilter
    {
        private readonly Lazy<AttachmentsSampleSystemServiceEnvironment> environment;

        private readonly IDashboardAuthorizationFilter baseFilter;

        public AttachmentsSampleSystemHangfireAuthorization(Lazy<AttachmentsSampleSystemServiceEnvironment> environment)
        {
            this.baseFilter = new AdminHangfireAuthorization<IAttachmentsSampleSystemBLLContext>(LazyHelper.Create(() => (IServiceEnvironment<IAttachmentsSampleSystemBLLContext>)this.environment.Value));

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
