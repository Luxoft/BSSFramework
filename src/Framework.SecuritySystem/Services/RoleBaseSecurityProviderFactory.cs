using Framework.Core;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public class RoleBaseSecurityProviderFactory<TDomainObject>(
    ISecurityFilterFactory<TDomainObject> securityFilterFactory,
    IAccessorsFilterFactory<TDomainObject> accessorsFilterFactory,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    ISecurityPathRestrictionService securityPathRestrictionService) : IRoleBaseSecurityProviderFactory<TDomainObject>
{
    public virtual ISecurityProvider<TDomainObject> Create(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        return this.GetRegroupedRoles(securityRule)
                   .Select(g => this.Create(g.SecurityRule, securityPath, g.Restriction)).Or();
    }

    private ISecurityProvider<TDomainObject> Create(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestriction restriction)
    {
        return new RoleBaseSecurityPathProvider<TDomainObject>(
            securityFilterFactory,
            accessorsFilterFactory,
            securityRule,
            securityPathRestrictionService.ApplyRestriction(securityPath, restriction));
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
