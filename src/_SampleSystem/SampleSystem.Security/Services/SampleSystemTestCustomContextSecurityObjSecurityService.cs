using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : ContextDomainSecurityServiceBase<TestCustomContextSecurityObj, Guid>
{
    public SampleSystemTestCustomContextSecurityObjSecurityService(
        ISecurityProvider<TestCustomContextSecurityObj> disabledSecurityProvider,
        ISecurityOperationResolver securityOperationResolver,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
        : base(disabledSecurityProvider, securityOperationResolver, securityExpressionBuilderFactory)
    {
    }

    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule securityRule)
    {
        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
