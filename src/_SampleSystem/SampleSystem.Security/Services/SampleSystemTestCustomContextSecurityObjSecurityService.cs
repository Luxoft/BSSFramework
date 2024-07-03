using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService(
    ISecurityProvider<TestCustomContextSecurityObj> disabledSecurityProvider)
    : DomainSecurityServiceBase<TestCustomContextSecurityObj>(disabledSecurityProvider)
{
    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule securityRule)
    {
        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
