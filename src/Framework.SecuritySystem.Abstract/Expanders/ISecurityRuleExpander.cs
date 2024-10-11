namespace Framework.SecuritySystem.Expanders;

public interface ISecurityRuleExpander : ISecurityModeExpander, ISecurityOperationExpander, ISecurityRoleExpander, IRoleFactorySecurityRuleExpander
{
    DomainSecurityRule.ExpandedRolesSecurityRule FullRoleExpand(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    DomainSecurityRule FullDomainExpand(DomainSecurityRule securityRule);
}
