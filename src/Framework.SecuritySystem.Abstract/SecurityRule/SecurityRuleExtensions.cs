#nullable enable
using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(this SecurityOperation securityOperation)
    {
        return new SecurityRule.OperationSecurityRule(securityOperation) { ExpandType = securityOperation.ExpandType };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this IEnumerable<SecurityRole> securityRoles, HierarchicalExpandType expandType = HierarchicalExpandType.Children)
    {
        return new SecurityRule.NonExpandedRolesSecurityRule(new DeepEqualsCollection<SecurityRole>(securityRoles.OrderBy(sr => sr.Name))) { ExpandType = expandType };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this SecurityRole securityRole, HierarchicalExpandType expandType = HierarchicalExpandType.Children)
    {
        return new[] { securityRole }.ToSecurityRule(expandType);
    }
}
