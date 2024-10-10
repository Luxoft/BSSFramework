namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IDomainSecurityRoleExtractor
{
    IEnumerable<SecurityRole> Extract(DomainSecurityRule securityRule);
}
