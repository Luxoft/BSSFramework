using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class SampleSystemTestCustomContextSecurityObjSecurityService : ContextDomainSecurityServiceBase<
    PersistentDomainObjectBase, TestCustomContextSecurityObj, Guid>
{
    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(ContextSecurityOperation securityOperation)
    {
        return new ConstSecurityProvider<TestCustomContextSecurityObj>(false);
    }

    public SampleSystemTestCustomContextSecurityObjSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver<PersistentDomainObjectBase> securityOperationResolver,
        IAuthorizationSystem<Guid> authorizationSystem,
        ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory)
        : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
    {
    }
}
