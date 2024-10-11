namespace Framework.SecuritySystem.SecurityRuleInfo;

public class ClientDomainModeSecurityRuleSource(IEnumerable<DomainModeSecurityRuleInfo> domainModeSecurityRuleInfoList)
    : IClientDomainModeSecurityRuleSource
{
    public IEnumerable<DomainSecurityRule.DomainModeSecurityRule> GetRules() =>
        domainModeSecurityRuleInfoList.Where(this.Allowed).Select(info => info.SecurityRule);

    protected virtual bool Allowed(DomainModeSecurityRuleInfo info) => info.Implementation != SecurityRule.Disabled;
}
