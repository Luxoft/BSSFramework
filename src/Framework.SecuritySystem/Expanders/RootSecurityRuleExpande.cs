using Framework.SecuritySystem.Services;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Expanders;

public class RootSecurityRuleExpander(
    ISecurityModeExpander securityModeExpander,
    ISecurityOperationExpander securityOperationExpander,
    ISecurityRoleExpander securityRoleExpander,
    IRoleFactorySecurityRuleExpander roleFactorySecurityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    ISecurityRuleImplementationResolver implementationResolver)
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

    public ExpandedRolesSecurityRule FullRoleExpand(RoleBaseSecurityRule securityRule)
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
                return this.FullRoleExpand(this.Expand(dynamicRoleSecurityRule));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    public DomainSecurityRule FullDomainExpand(DomainSecurityRule securityRule)
    {
        return new FullDomainExpandVisitor(this, implementationResolver).Visit(securityRule);
    }

    private class FullDomainExpandVisitor(ISecurityRuleExpander expander, ISecurityRuleImplementationResolver implementationResolver)
        : SecurityRuleVisitor
    {
        protected override DomainSecurityRule Visit(SecurityRuleHeader securityRule)
        {
            return this.Visit(implementationResolver.Resolve(securityRule));
        }

        protected override DomainSecurityRule Visit(RoleBaseSecurityRule baseSecurityRule) => expander.FullRoleExpand(baseSecurityRule);

        protected override DomainSecurityRule Visit(DomainModeSecurityRule securityRule) => this.Visit(expander.Expand(securityRule));
    }
}
