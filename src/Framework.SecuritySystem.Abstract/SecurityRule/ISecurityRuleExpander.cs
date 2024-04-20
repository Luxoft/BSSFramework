#nullable enable

namespace Framework.SecuritySystem;

public interface ISecurityRuleExpander
{
    SecurityRule? TryExpand<TDomainObject>(SecurityRule.SpecialSecurityRule securityRule);

    SecurityRule.NonExpandedRolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule);

    SecurityRule.ExpandedRolesSecurityRule Expand(SecurityRule.NonExpandedRolesSecurityRule securityRule);

    IEnumerable<SecurityRule.ExpandedRolesSecurityRule> FullExpand(SecurityRule.DomainObjectSecurityRule securityRule);
}
