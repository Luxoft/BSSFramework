﻿namespace Framework.SecuritySystem.Expanders;

public interface ISecurityRuleExpander
{
    SecurityRule.DomainSecurityRule? TryExpand<TDomainObject>(SecurityRule.ModeSecurityRule securityRule);

    SecurityRule.NonExpandedRolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule);

    SecurityRule.ExpandedRolesSecurityRule Expand(SecurityRule.NonExpandedRolesSecurityRule securityRule);

    SecurityRule.RoleBaseSecurityRule Expand(SecurityRule.DynamicRoleSecurityRule securityRule);

    IEnumerable<SecurityRule.ExpandedRolesSecurityRule> FullExpand(SecurityRule.RoleBaseSecurityRule securityRule);
}
