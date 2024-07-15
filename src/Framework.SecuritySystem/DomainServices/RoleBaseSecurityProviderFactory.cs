using Framework.Core;
using Framework.SecuritySystem.Providers.Operation;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem;

public class RoleBaseSecurityProviderFactory(
    ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    ISecurityPathRestrictionService securityPathRestrictionService)
{
    public virtual ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.GetRegroupedRoles(securityRule)
                   .Select(g => this.Create(securityPath, g.SecurityRule, g.Restriction)).Or();
    }
    private ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPathRestriction restriction)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPathRestrictionService.ApplyRestriction(securityPath, restriction),
            securityRule,
            securityExpressionBuilderFactory);
    }

    private IEnumerable<(SecurityRule.ExpandedRolesSecurityRule SecurityRule, SecurityPathRestriction Restriction)> GetRegroupedRoles(
        SecurityRule.RoleBaseSecurityRule securityRule)
    {
        return from expandedSecurityRule in securityRuleExpander.FullExpand(securityRule)

               from securityRole in expandedSecurityRule.SecurityRoles

               let securityRoleInfo = securityRoleSource.GetSecurityRole(securityRole).Information

               let actualCustomExpandType = securityRule.CustomExpandType
                                            ?? expandedSecurityRule.CustomExpandType ?? securityRoleInfo.CustomExpandType

               group securityRole by new { actualCustomExpandType, securityRoleInfo.Restriction } into g

               let rule = new SecurityRule.ExpandedRolesSecurityRule(DeepEqualsCollection.Create(g))
                          {
                              CustomExpandType = g.Key.actualCustomExpandType
                          }

               select (rule, g.Key.Restriction);
    }
}
