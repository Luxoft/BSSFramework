#nullable enable

using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public class SecurityRuleExpander(
    SecurityModeExpander securityModeExpander,
    SecurityOperationExpander securityOperationExpander,
    SecurityRoleExpander securityRoleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRuleExpander
{
    public SecurityRule? TryExpand<TDomainObject>(SecurityRule.SpecialSecurityRule securityRule)
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

    public IEnumerable<SecurityRule.ExpandedRolesSecurityRule> FullExpand(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.OperationSecurityRule operationSecurityRule:
                return [this.Expand(this.Expand(operationSecurityRule))];

            case SecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return [this.Expand(nonExpandedRolesSecurityRule)];

            case SecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return [expandedRolesSecurityRule];

            case SecurityRule.CompositeSecurityRule compositeSecurityRule:
                return compositeSecurityRule.Children.SelectMany(this.FullExpand).Distinct();

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }

    public HierarchicalExpandType? TryGetCustomExpandType<TDomainObject>(SecurityRule.SpecialSecurityRule securityRule)
    {
        switch (this.TryExpand<TDomainObject>(securityRule))
        {
            case SecurityRule.OperationSecurityRule operationSecurityRule:
                return operationSecurityRule.CustomExpandType ?? securityOperationExpander.Expand(operationSecurityRule).CustomExpandType;

            case SecurityRule.NonExpandedRolesSecurityRule { SecurityRoles: [var securityRole] } nonExpandedRolesSecurityRule:
                return nonExpandedRolesSecurityRule.CustomExpandType
                       ?? securityRoleSource.GetFullRole(securityRole).Information.CustomExpandType;

            case SecurityRule.DomainObjectSecurityRule domainObjectSecurityRule:
                return domainObjectSecurityRule.CustomExpandType;

            default:
                return null;
        }
    }
}
