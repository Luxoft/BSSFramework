namespace Framework.SecuritySystem.Expanders;

public class SecurityRuleExpander(
    SecurityModeExpander securityModeExpander,
    SecurityOperationExpander securityOperationExpander,
    SecurityRoleExpander securityRoleExpander)
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

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }
}
