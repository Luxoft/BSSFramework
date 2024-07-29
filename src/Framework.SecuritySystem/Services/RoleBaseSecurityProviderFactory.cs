using Framework.Core;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Providers.Operation;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.Services;

public class RoleBaseSecurityProviderFactory(
    ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    ISecurityPathRestrictionService securityPathRestrictionService) : IRoleBaseSecurityProviderFactory
{
    public virtual ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.GetRegroupedRoles(securityRule)
                   .Select(g => this.Create(securityPath, g.SecurityRule, g.Restriction)).Or();
    }

    private ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPathRestriction restriction)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPathRestrictionService.ApplyRestriction(securityPath, restriction),
            securityRule,
            securityExpressionBuilderFactory);
    }

    private IEnumerable<(DomainSecurityRule.ExpandedRolesSecurityRule SecurityRule, SecurityPathRestriction Restriction)> GetRegroupedRoles(
        DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var expandedSecurityRule = securityRuleExpander.FullExpand(securityRule);

        return from securityRole in expandedSecurityRule.SecurityRoles

               let securityRoleInfo = securityRoleSource.GetSecurityRole(securityRole).Information

               let actualCustomExpandType = securityRule.CustomExpandType
                                            ?? expandedSecurityRule.CustomExpandType ?? securityRoleInfo.CustomExpandType

               group securityRole by new { actualCustomExpandType, securityRoleInfo.Restriction } into g

               let rule = new DomainSecurityRule.ExpandedRolesSecurityRule(DeepEqualsCollection.Create(g))
               {
                   CustomExpandType = g.Key.actualCustomExpandType
               }

               select (rule, g.Key.Restriction);
    }
}
