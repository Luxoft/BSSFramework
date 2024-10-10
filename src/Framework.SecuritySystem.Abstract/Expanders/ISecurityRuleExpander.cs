namespace Framework.SecuritySystem.Expanders;

public interface ISecurityRuleExpander : ISecurityModeExpander, ISecurityOperationExpander, ISecurityRoleExpander, IRoleFactorySecurityRuleExpander
{
    DomainSecurityRule.ExpandedRolesSecurityRule FullExpand(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
