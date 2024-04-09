#nullable enable
using Framework.Core;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(this SecurityOperation securityOperation)
    {
        return new SecurityRule.OperationSecurityRule(securityOperation) { ExpandType = securityOperation.ExpandType };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this IEnumerable<SecurityRole> securityRoles)
    {
        return new SecurityRule.NonExpandedRolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles.OrderBy(sr => sr.Name)));
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this SecurityRole securityRole)
    {
        return new[] { securityRole }.ToSecurityRule();
    }
}
