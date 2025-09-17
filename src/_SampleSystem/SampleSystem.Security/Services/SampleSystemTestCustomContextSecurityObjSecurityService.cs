using SecuritySystem;

using SampleSystem.Domain;

using SecuritySystem.Providers;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : DomainSecurityServiceBase<TestCustomContextSecurityObj>
{
    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule securityRule)
    {
        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
