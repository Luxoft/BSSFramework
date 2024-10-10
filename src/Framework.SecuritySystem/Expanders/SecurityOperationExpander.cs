using Framework.Core;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Expanders;

public class SecurityOperationExpander(ISecurityRoleSource securityRoleSource, ISecurityOperationInfoSource securityOperationInfoSource)
    : ISecurityOperationExpander
{
    private readonly IDictionaryCache<OperationSecurityRule, NonExpandedRolesSecurityRule> cache =
        new DictionaryCache<OperationSecurityRule, NonExpandedRolesSecurityRule>(
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


    public NonExpandedRolesSecurityRule Expand(OperationSecurityRule securityRule)
    {
        return this.cache[securityRule];
    }
}
