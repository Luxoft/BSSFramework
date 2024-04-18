#nullable enable

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(this SecurityOperation securityOperation, SecurityRuleRestriction? restriction = null)
    {
        return new SecurityRule.OperationSecurityRule(securityOperation) { ExpandType = securityOperation.ExpandType, Restriction = restriction };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this IEnumerable<SecurityRole> securityRoles, HierarchicalExpandType expandType = HierarchicalExpandType.Children, SecurityRuleRestriction? restriction = null)
    {
        return new SecurityRule.NonExpandedRolesSecurityRule(DeepEqualsCollection.Create(securityRoles.OrderBy(sr => sr.Name))) { ExpandType = expandType, Restriction = restriction };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(this SecurityRole securityRole, HierarchicalExpandType expandType = HierarchicalExpandType.Children, SecurityRuleRestriction? restriction = null)
    {
        return new[] { securityRole }.ToSecurityRule(expandType, restriction);
    }
}
