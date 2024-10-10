namespace Framework.SecuritySystem.SecurityRuleInfo;

public class ClientDomainModeSecurityRuleSource(IEnumerable<DomainModeSecurityRuleInfo> domainModeSecurityRuleInfoList)
    : IClientDomainModeSecurityRuleSource
{
    public IEnumerable<DomainSecurityRule.DomainModeSecurityRule> GetRules() =>
        domainModeSecurityRuleInfoList
            .Where(info => info.Implementation != SecurityRule.Disabled)
            .Select(info => info.SecurityRule);
}
