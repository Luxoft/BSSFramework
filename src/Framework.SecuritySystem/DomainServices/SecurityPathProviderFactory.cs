using Framework.Core;
using Framework.SecuritySystem.Providers.Operation;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem;

public class SecurityPathProviderFactory(
    ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    ISecurityPathRestrictionService securityPathRestrictionService) : ISecurityPathProviderFactory
{
    public ISecurityProvider<TDomainObject> Create<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityRule.DomainObjectSecurityRule rootSecurityRule)
    {
        return this.GetRegroupedRoles(rootSecurityRule).Select(g => this.Create(securityPath, g.SecurityRule, g.Restriction)).Or();
    }

    private ISecurityProvider<TDomainObject> Create<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityRule.ExpandedRolesSecurityRule securityRule, SecurityPathRestriction restriction)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPathRestrictionService.ApplyRestriction(securityPath, restriction),
            securityRule,
            securityExpressionBuilderFactory);
    }

    private IEnumerable<(SecurityRule.ExpandedRolesSecurityRule SecurityRule, SecurityPathRestriction Restriction)> GetRegroupedRoles(SecurityRule.DomainObjectSecurityRule rootSecurityRule)
    {
        return from expandedSecurityRule in securityRuleExpander.FullExpand(rootSecurityRule)

               from securityRole in expandedSecurityRule.SecurityRoles

               let securityRoleInfo = securityRoleSource.GetFullRole(securityRole).Information

               let actualExpandType = rootSecurityRule.CustomExpandType ?? expandedSecurityRule.CustomExpandType ?? securityRoleInfo.ExpandType

               group securityRole by new { actualExpandType, securityRoleInfo.Restriction } into g

               let rule = new SecurityRule.ExpandedRolesSecurityRule(DeepEqualsCollection.Create(g)) { CustomExpandType = g.Key.actualExpandType }

               select (rule, g.Key.Restriction);
    }
}
