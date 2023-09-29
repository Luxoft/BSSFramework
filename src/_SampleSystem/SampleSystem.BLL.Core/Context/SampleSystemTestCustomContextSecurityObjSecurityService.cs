using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class SampleSystemTestCustomContextSecurityObjSecurityService : ContextDomainSecurityServiceBase<TestCustomContextSecurityObj, Guid>
{
    public SampleSystemTestCustomContextSecurityObjSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IAuthorizationSystem<Guid> authorizationSystem,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
        : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
    {
    }

    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
