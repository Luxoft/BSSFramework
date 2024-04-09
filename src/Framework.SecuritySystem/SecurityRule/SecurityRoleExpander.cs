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
                var securityRoles = securityRoleSource.SecurityRoles
                                                      .Distinct()
                                                      .Where(sr => sr.Children.IsIntersected(securityRule.SecurityRoles))
                                                      .Concat(securityRoleSource.SecurityRoles)
                                                      .Distinct()
                                                      .OrderBy(sr => sr.Name)
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
