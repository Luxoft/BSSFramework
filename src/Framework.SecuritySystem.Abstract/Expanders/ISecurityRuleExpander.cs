namespace Framework.SecuritySystem.Expanders;

public interface ISecurityRuleExpander
{
    DomainSecurityRule? TryExpand<TDomainObject>(SecurityRule.ModeSecurityRule securityRule);

    DomainSecurityRule.NonExpandedRolesSecurityRule Expand(DomainSecurityRule.OperationSecurityRule securityRule);

    DomainSecurityRule.ExpandedRolesSecurityRule Expand(DomainSecurityRule.NonExpandedRolesSecurityRule securityRule);

    DomainSecurityRule.RoleBaseSecurityRule Expand(DomainSecurityRule.RoleFactorySecurityRule securityRule);

    IEnumerable<DomainSecurityRule.ExpandedRolesSecurityRule> FullExpand(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
