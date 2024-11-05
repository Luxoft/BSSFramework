namespace Framework.SecuritySystem.Expanders;

public interface ISecurityRuleExpander : ISecurityModeExpander,
                                         ISecurityOperationExpander,
                                         ISecurityRoleExpander,
                                         IRoleFactorySecurityRuleExpander,
                                         IClientSecurityRuleExpander,
                                         ISecurityRuleHeaderExpander
{
    DomainSecurityRule.ExpandedRolesSecurityRule FullRoleExpand(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    DomainSecurityRule FullDomainExpand(DomainSecurityRule securityRule, SecurityRuleExpandSettings? settings = null);
}
