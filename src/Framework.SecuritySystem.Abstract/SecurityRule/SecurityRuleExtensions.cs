#nullable enable
using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(this SecurityOperation securityOperation)
    {
        return new SecurityRule.OperationSecurityRule(securityOperation);
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this IEnumerable<SecurityRole> securityRoles, HierarchicalExpandType? customExpandType = null)
    {
        return new SecurityRule.NonExpandedRolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles.OrderBy(sr => sr.Name))) { CustomExpandType = customExpandType };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this SecurityRole securityRole, HierarchicalExpandType? customExpandType = null)
    {
        return new[] { securityRole }.ToSecurityRule(customExpandType);
    }
}
