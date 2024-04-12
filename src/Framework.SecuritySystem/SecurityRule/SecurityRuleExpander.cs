#nullable enable
namespace Framework.SecuritySystem;

public class SecurityRuleExpander : ISecurityRuleExpander
{
    private readonly SecurityModeExpander securityModeExpander;

    private readonly SpecialRoleSecurityRuleExpander specialRoleSecurityRuleExpander;

    private readonly SecurityOperationExpander securityOperationExpander;

    private readonly SecurityRoleExpander securityRoleExpander;

    public SecurityRuleExpander(
        SecurityModeExpander securityModeExpander,
        SpecialRoleSecurityRuleExpander specialRoleSecurityRuleExpander,
        SecurityOperationExpander securityOperationExpander,
        SecurityRoleExpander securityRoleExpander)
    {
        this.securityModeExpander = securityModeExpander;
        this.securityOperationExpander = securityOperationExpander;
        this.securityRoleExpander = securityRoleExpander;
        this.specialRoleSecurityRuleExpander = specialRoleSecurityRuleExpander;
    }

    public SecurityRule? TryExpand<TDomainObject>(SecurityRule.SpecialSecurityRule securityRule)
    {
        return this.securityModeExpander.TryExpand<TDomainObject>(securityRule);
    }

    public SecurityRule.NonExpandedRolesSecurityRule Expand(SpecialRoleSecurityRule securityRule)
    {
        return this.specialRoleSecurityRuleExpander.Expand(securityRule);
    }

    public SecurityRule.NonExpandedRolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.securityOperationExpander.Expand(securityRule);
    }

    public SecurityRule.ExpandedRolesSecurityRule Expand(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.securityRoleExpander.Expand(securityRule);
    }

    public SecurityRule.ExpandedRolesSecurityRule FullExpand(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SpecialRoleSecurityRule specialRoleSecurityRule:
                return this.Expand(this.Expand(specialRoleSecurityRule));

            case SecurityRule.OperationSecurityRule operationSecurityRule:
                return this.Expand(this.Expand(operationSecurityRule));

            case SecurityRule.NonExpandedRolesSecurityRule nonExpandedRolesSecurityRule:
                return this.Expand(nonExpandedRolesSecurityRule);

            case SecurityRule.ExpandedRolesSecurityRule expandedRolesSecurityRule:
                return expandedRolesSecurityRule;

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }
}
