using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemTestCustomContextSecurityObjSecurityService
    {
        protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(ContextSecurityOperation<WorkflowSampleSystemSecurityOperationCode> securityOperation)
        {
            return new ConstSecurityProvider<TestCustomContextSecurityObj>(this.AccessDeniedExceptionService, false);
        }
    }
}
