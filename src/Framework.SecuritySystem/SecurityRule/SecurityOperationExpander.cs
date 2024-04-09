#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityOperationExpander
{
    private readonly IDictionaryCache<SecurityRule.OperationSecurityRule, SecurityRule.NonExpandedRolesSecurityRule> expandCache;

    public SecurityOperationExpander(ISecurityRoleSource securityRoleSource)
    {
        this.expandCache = new DictionaryCache<SecurityRule.OperationSecurityRule, SecurityRule.NonExpandedRolesSecurityRule>(
            securityRule =>
            {
                var securityRoles = securityRoleSource.SecurityRoles
                                                      .Where(sr => sr.Operations.Contains(securityRule.SecurityOperation))
                                                      .Distinct()
                                                      .ToArray();

                if (securityRoles.Length == 0)
                {
                    throw new Exception("The list of security roles cannot be empty!");
                }

                return securityRoles.ToSecurityRule();
            }).WithLock();
    }

    public SecurityRule.NonExpandedRolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }
}
