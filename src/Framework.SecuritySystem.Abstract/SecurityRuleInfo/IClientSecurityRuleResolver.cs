namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IClientSecurityRuleResolver
{
    IEnumerable<ClientSecurityRuleHeader> Resolve(SecurityRole securityRole);
}
