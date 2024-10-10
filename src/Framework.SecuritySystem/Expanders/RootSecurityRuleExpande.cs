using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Expanders;

public class RootSecurityRuleExpander(
    ISecurityModeExpander securityModeExpander,
    ISecurityOperationExpander securityOperationExpander,
    ISecurityRoleExpander securityRoleExpander,
    IRoleFactorySecurityRuleExpander roleFactorySecurityRuleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRuleExpander
{
    public DomainSecurityRule? TryExpand(DomainModeSecurityRule securityRule)
    {
        return securityModeExpander.TryExpand(securityRule);
    }

    public NonExpandedRolesSecurityRule Expand(OperationSecurityRule securityRule)
    {
        return securityOperationExpander.Expand(securityRule);
    }

    public ExpandedRolesSecurityRule Expand(NonExpandedRolesSecurityRule securityRule)
    {
        return securityRoleExpander.Expand(securityRule);
    }

    public RoleBaseSecurityRule Expand(RoleFactorySecurityRule securityRule)
    {
        return roleFactorySecurityRuleExpander.Expand(securityRule);
    }

    public ExpandedRolesSecurityRule FullExpand(RoleBaseSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case AnyRoleSecurityRule:
                return ExpandedRolesSecurityRule.Create(securityRoleSource.SecurityRoles);

            case OperationSecurityRule operationSecurityRule:
                return this.Expand(this.Expand(operationSecurityRule));

            case NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return this.Expand(nonExpandedRolesSecurityRule);

            case ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return expandedRolesSecurityRule;

            case RoleFactorySecurityRule dynamicRoleSecurityRule:
                return this.FullExpand(this.Expand(dynamicRoleSecurityRule));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }
}
