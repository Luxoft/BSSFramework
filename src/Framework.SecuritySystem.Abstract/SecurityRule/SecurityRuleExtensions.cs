#nullable enable
using Framework.Core;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(this SecurityOperation securityOperation)
    {
        return new SecurityRule.OperationSecurityRule(securityOperation) { ExpandType = securityOperation.ExpandType };
    }

    public static SecurityRule.RolesSecurityRule ToSecurityRule(this IEnumerable<SecurityRole> securityRoles)
    {
        return new SecurityRule.RolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles));
    }

    public static SecurityRule.RolesSecurityRule ToSecurityRule(this SecurityRole securityRole)
    {
        return new[] { securityRole }.ToSecurityRule();
    }
}
