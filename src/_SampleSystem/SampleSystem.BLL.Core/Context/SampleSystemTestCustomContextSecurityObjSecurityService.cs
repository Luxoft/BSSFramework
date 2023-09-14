using Framework.SecuritySystem;
using Framework.SecuritySystem;
using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class SampleSystemTestCustomContextSecurityObjSecurityService
{
    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(ContextSecurityOperation securityOperation)
    {
        return new ConstSecurityProvider<TestCustomContextSecurityObj>(false);
    }
}
