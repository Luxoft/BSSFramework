using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : ContextDomainSecurityServiceBase<TestCustomContextSecurityObj, Guid>
{
    public SampleSystemTestCustomContextSecurityObjSecurityService(
        ISecurityProvider<TestCustomContextSecurityObj> disabledSecurityProvider,
        IEnumerable<ISecurityRuleExpander> securityRuleExpanders,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
        : base(disabledSecurityProvider, securityRuleExpanders, securityExpressionBuilderFactory)
    {
    }

    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
