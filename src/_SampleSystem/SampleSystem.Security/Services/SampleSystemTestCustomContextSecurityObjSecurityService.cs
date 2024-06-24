using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : DomainSecurityServiceBase<TestCustomContextSecurityObj>
{
    public SampleSystemTestCustomContextSecurityObjSecurityService(
        ISecurityProvider<TestCustomContextSecurityObj> disabledSecurityProvider)
        : base(disabledSecurityProvider)
    {
    }

    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule securityRule)
    {
        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
