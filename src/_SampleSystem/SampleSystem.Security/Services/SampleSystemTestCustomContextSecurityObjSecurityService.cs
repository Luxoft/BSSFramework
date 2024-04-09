using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class SampleSystemTestCustomContextSecurityObjSecurityService : ContextDomainSecurityServiceBase<TestCustomContextSecurityObj, Guid>
{
    public SampleSystemTestCustomContextSecurityObjSecurityService(
        ISecurityProvider<TestCustomContextSecurityObj> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
        : base(disabledSecurityProvider, securityRuleExpander, securityExpressionBuilderFactory)
    {
    }

    protected override ISecurityProvider<TestCustomContextSecurityObj> CreateSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        if (securityRule == SecurityRule.View)
        {
        }

        if (securityRule == SampleSystemSecurityOperation.BusinessUnitEdit)
        {
        }

        if (securityRule == SampleSystemSecurityRole.SeManager)
        {
        }

        if (securityRule is SecurityRule.ExpandedRolesSecurityRule final && final.SecurityRoles.Contains(SampleSystemSecurityRole.SeManager))
        {
        }

        return new AccessDeniedSecurityProvider<TestCustomContextSecurityObj>();
    }
}
