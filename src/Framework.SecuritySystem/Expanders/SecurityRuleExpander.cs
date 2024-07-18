namespace Framework.SecuritySystem.Expanders;

public class SecurityRuleExpander(
    SecurityModeExpander securityModeExpander,
    SecurityOperationExpander securityOperationExpander,
    SecurityRoleExpander securityRoleExpander,
    DynamicRoleSecurityRuleExpander dynamicRoleSecurityRuleExpander)
    : ISecurityRuleExpander
{
    public SecurityRule.DomainSecurityRule? TryExpand<TDomainObject>(SecurityRule.ModeSecurityRule securityRule)
    {
        return securityModeExpander.TryExpand<TDomainObject>(securityRule);
    }

    public SecurityRule.NonExpandedRolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule)
    {
        return securityOperationExpander.Expand(securityRule);
    }

    public SecurityRule.ExpandedRolesSecurityRule Expand(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return securityRoleExpander.Expand(securityRule);
    }

    public SecurityRule.RoleBaseSecurityRule Expand(SecurityRule.DynamicRoleSecurityRule securityRule)
    {
        return dynamicRoleSecurityRuleExpander.Expand(securityRule);
    }

    public IEnumerable<SecurityRule.ExpandedRolesSecurityRule> FullExpand(SecurityRule.RoleBaseSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.OperationSecurityRule operationSecurityRule:
                return [this.Expand(this.Expand(operationSecurityRule))];

            case SecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return [this.Expand(nonExpandedRolesSecurityRule)];

            case SecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return [expandedRolesSecurityRule];

            case SecurityRule.DynamicRoleSecurityRule dynamicRoleSecurityRule:
                return this.FullExpand(this.Expand(dynamicRoleSecurityRule));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }
}
