#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityRoleExpander : ISecurityRuleExpander
{
    private readonly IDictionaryCache<SecurityRule, SecurityRule.RolesSecurityRule?> tryExpandCache;

    private readonly IDictionaryCache<SecurityRule.RolesSecurityRule, SecurityRule.RolesSecurityRule> expandCache;

    public SecurityRoleExpander(ISecurityRoleSource securityRoleSource)
    {
        this.tryExpandCache = new DictionaryCache<SecurityRule, SecurityRule.RolesSecurityRule?>(
            securityRule =>
            {
                if (securityRule is SecurityRule.RolesSecurityRule rolesSecurityRule)
                {
                    var expandedSecurityRule = this.Expand(rolesSecurityRule);

                    if (expandedSecurityRule != securityRule)
                    {
                        return expandedSecurityRule;
                    }
                }

                return null;
            }).WithLock();

        this.expandCache = new DictionaryCache<SecurityRule.RolesSecurityRule, SecurityRule.RolesSecurityRule>(
            securityRule =>
            {
                var securityRoles = securityRoleSource.SecurityRoles.GetAllElements(sr => sr.Children)
                                        .Distinct()
                                        .Where(sr => sr.Children.IsIntersected(securityRule.SecurityRoles))
                                        .ToArray();

                if (securityRoles.Length == 0)
                {
                    throw new Exception("The list of security roles cannot be empty!");
                }

                return securityRoles.ToSecurityRule();
            }).WithLock();
    }

    public SecurityRule.RolesSecurityRule Expand(SecurityRule.RolesSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }

    public SecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule)
    {
        return this.tryExpandCache[securityRule];
    }
}
