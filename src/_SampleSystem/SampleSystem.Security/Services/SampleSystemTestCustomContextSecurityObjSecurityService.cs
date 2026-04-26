using Anch.SecuritySystem;

using SampleSystem.Domain;

using Anch.SecuritySystem.DomainServices;
using Anch.SecuritySystem.Providers;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : DomainSecurityServiceBase<TestCustomContextSecurityObj>
{
    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule securityRule) => new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
}
