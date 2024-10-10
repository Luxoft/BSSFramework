namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IDomainModeSecurityRuleResolver
{
    IEnumerable<DomainSecurityRule.DomainModeSecurityRule> Resolve(SecurityRole securityRole);
}
