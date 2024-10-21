using Framework.SecuritySystem.Services;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Expanders;

public class RootSecurityRuleExpander(
    ISecurityModeExpander securityModeExpander,
    ISecurityOperationExpander securityOperationExpander,
    ISecurityRoleExpander securityRoleExpander,
    IRoleFactorySecurityRuleExpander roleFactorySecurityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    IClientSecurityRuleExpander clientSecurityRuleExpander,
    ISecurityRuleHeaderExpander securityRuleHeaderExpander)
    : ISecurityRuleExpander
{
    public DomainSecurityRule? TryExpand(DomainModeSecurityRule securityRule)
    {
        return securityModeExpander.TryExpand(securityRule);
    }

    public DomainSecurityRule Expand(SecurityRuleHeader securityRuleHeader) => securityRuleHeaderExpander.Expand(securityRuleHeader);

    public DomainSecurityRule Expand(ClientSecurityRule securityRule)
    {
        return clientSecurityRuleExpander.Expand(securityRule);
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

                return ExpandedRolesSecurityRule.Create(securityRoleSource.SecurityRoles).WithCopyCustoms(securityRule);

            case RoleGroupSecurityRule roleGroupSecurityRule:

                return ExpandedRolesSecurityRule
                       .Create(roleGroupSecurityRule.Children.SelectMany(c => this.FullRoleExpand(c).SecurityRoles))
                       .WithCopyCustoms(securityRule);

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
        return new FullDomainExpandVisitor(this).Visit(securityRule);
    }

    private class FullDomainExpandVisitor(ISecurityRuleExpander expander)
        : SecurityRuleVisitor
    {
        protected override DomainSecurityRule Visit(RoleBaseSecurityRule baseSecurityRule) => expander.FullRoleExpand(baseSecurityRule);

        protected override DomainSecurityRule Visit(DomainModeSecurityRule securityRule) => this.Visit(expander.Expand(securityRule));

        protected override DomainSecurityRule Visit(SecurityRuleHeader securityRule) => this.Visit(expander.Expand(securityRule));

        protected override DomainSecurityRule Visit(ClientSecurityRule securityRule) => this.Visit(expander.Expand(securityRule));
    }
}
