namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IClientSecurityRuleResolver
{
    IEnumerable<DomainSecurityRule.ClientSecurityRule> Resolve(SecurityRole securityRole);
}
