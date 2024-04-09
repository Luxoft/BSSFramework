#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityRoleExpander
{
    private readonly IDictionaryCache<SecurityRule.NonExpandedRolesSecurityRule, SecurityRule.ExpandedRolesSecurityRule> expandCache;

    public SecurityRoleExpander(ISecurityRoleSource securityRoleSource)
    {
        this.expandCache = new DictionaryCache<SecurityRule.NonExpandedRolesSecurityRule, SecurityRule.ExpandedRolesSecurityRule>(
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

                return new SecurityRule.ExpandedRolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles));
            }).WithLock();
    }

    public SecurityRule.ExpandedRolesSecurityRule Expand(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }
}
