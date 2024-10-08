using Framework.Core;

namespace Framework.SecuritySystem.Expanders;

public class SecurityOperationExpander
{
    private readonly IDictionaryCache<DomainSecurityRule.OperationSecurityRule, DomainSecurityRule.NonExpandedRolesSecurityRule> expandCache;

    public SecurityOperationExpander(ISecurityRoleSource securityRoleSource, ISecurityOperationInfoSource securityOperationInfoSource)
    {
        this.expandCache = new DictionaryCache<DomainSecurityRule.OperationSecurityRule, DomainSecurityRule.NonExpandedRolesSecurityRule>(
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
                    ?? securityOperationInfoSource.GetSecurityOperationInfo(securityRule.SecurityOperation).CustomExpandType,
                    securityRule.CustomCredential,
                    securityRule.CustomRestriction);

            }).WithLock();
    }

    public DomainSecurityRule.NonExpandedRolesSecurityRule Expand(DomainSecurityRule.OperationSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }
}
