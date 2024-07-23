namespace Framework.SecuritySystem.Expanders;

public class SecurityRuleExpander(
    SecurityModeExpander securityModeExpander,
    SecurityOperationExpander securityOperationExpander,
    SecurityRoleExpander securityRoleExpander,
    RoleFactorySecurityRuleExpander dynamicRoleSecurityRuleExpander)
    : ISecurityRuleExpander
{
    public DomainSecurityRule? TryExpand<TDomainObject>(SecurityRule.ModeSecurityRule securityRule)
    {
        return securityModeExpander.TryExpand<TDomainObject>(securityRule);
    }

    public DomainSecurityRule.NonExpandedRolesSecurityRule Expand(DomainSecurityRule.OperationSecurityRule securityRule)
    {
        return securityOperationExpander.Expand(securityRule);
    }

    public DomainSecurityRule.ExpandedRolesSecurityRule Expand(DomainSecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return securityRoleExpander.Expand(securityRule);
    }

    public DomainSecurityRule.RoleBaseSecurityRule Expand(DomainSecurityRule.RoleFactorySecurityRule securityRule)
    {
        return dynamicRoleSecurityRuleExpander.Expand(securityRule);
    }

    public IEnumerable<DomainSecurityRule.ExpandedRolesSecurityRule> FullExpand(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case DomainSecurityRule.OperationSecurityRule operationSecurityRule:
                return [this.Expand(this.Expand(operationSecurityRule))];

            case DomainSecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return [this.Expand(nonExpandedRolesSecurityRule)];

            case DomainSecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return [expandedRolesSecurityRule];

            case DomainSecurityRule.RoleFactorySecurityRule dynamicRoleSecurityRule:
                return this.FullExpand(this.Expand(dynamicRoleSecurityRule));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }
}
