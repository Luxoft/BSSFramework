namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface ISecurityRuleParser
{
    SecurityRule Parse<TDomainObject>(string name);
}
