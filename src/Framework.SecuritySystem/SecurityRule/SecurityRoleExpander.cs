﻿#nullable enable

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
                if (securityRule.SecurityRoles.Count == 0)
                {
                    throw new Exception("The list of security roles cannot be empty.");
                }

                var securityRoles = securityRoleSource.SecurityRoles
                                                      .Where(sr => sr.GetAllElements(c => c.Children).IsIntersected(securityRule.SecurityRoles))
                                                      .Concat(securityRule.SecurityRoles)
                                                      .Distinct()
                                                      .OrderBy(sr => sr.Name)
                                                      .ToArray();

                return new SecurityRule.ExpandedRolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles)) { ExpandType = securityRule.ExpandType };
            }).WithLock();
    }

    public SecurityRule.ExpandedRolesSecurityRule Expand(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }
}
