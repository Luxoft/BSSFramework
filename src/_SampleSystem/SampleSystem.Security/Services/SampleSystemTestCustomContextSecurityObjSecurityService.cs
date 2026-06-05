using Anch.SecuritySystem;
using Anch.SecuritySystem.DomainServices;
using Anch.SecuritySystem.Providers;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : DomainSecurityServiceBase<TestCustomContextSecurityObj>
{
    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule securityRule) => new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
}

