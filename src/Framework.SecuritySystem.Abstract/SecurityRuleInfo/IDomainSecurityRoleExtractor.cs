namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IDomainSecurityRoleExtractor
{
    IEnumerable<SecurityRole> ExtractSecurityRoles(DomainSecurityRule securityRule);

    DomainSecurityRule.RoleBaseSecurityRule ExtractSecurityRule(DomainSecurityRule securityRule);
}
