using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityOperationExpander
{
    private readonly IDictionaryCache<SecurityRule.OperationSecurityRule, SecurityRule.NonExpandedRolesSecurityRule> expandCache;

    public SecurityOperationExpander(ISecurityRoleSource securityRoleSource, ISecurityOperationInfoSource securityOperationInfoSource)
    {
        this.expandCache = new DictionaryCache<SecurityRule.OperationSecurityRule, SecurityRule.NonExpandedRolesSecurityRule>(
            securityRule =>
            {
                var securityRoles = securityRoleSource.SecurityRoles
                                                      .Where(sr => sr.Information.Operations.Contains(securityRule.SecurityOperation))
                                                      .Distinct()
                                                      .ToArray();

                if (securityRoles.Length == 0)
                {
                    throw new Exception($"No security roles found for operation \"{securityRule.SecurityOperation}\"");
                }

                return securityRoles.ToSecurityRule(
                    securityRule.CustomExpandType
                    ?? securityOperationInfoSource.GetSecurityOperationInfo(securityRule.SecurityOperation).CustomExpandType);

            }).WithLock();
    }

    public SecurityRule.NonExpandedRolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }
}
