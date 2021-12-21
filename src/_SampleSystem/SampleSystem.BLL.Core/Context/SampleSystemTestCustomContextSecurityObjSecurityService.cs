using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.BLL
{
    public partial class SampleSystemTestCustomContextSecurityObjSecurityService
    {
        protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(ContextSecurityOperation<SampleSystemSecurityOperationCode> securityOperation)
        {
            return new ConstSecurityProvider<TestCustomContextSecurityObj>(this.AccessDeniedExceptionService, false);
        }
    }
}
