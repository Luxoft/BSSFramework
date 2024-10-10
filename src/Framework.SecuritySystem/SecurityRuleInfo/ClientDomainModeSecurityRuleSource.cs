namespace Framework.SecuritySystem.SecurityRuleInfo;

public class ClientDomainModeSecurityRuleSource(IEnumerable<DomainModeSecurityRuleInfo> domainModeSecurityRuleInfoList)
    : IClientDomainModeSecurityRuleSource
{
    public IEnumerable<DomainSecurityRule.DomainModeSecurityRule> GetRules() =>
        domainModeSecurityRuleInfoList.Select(info => info.SecurityRule);
}
