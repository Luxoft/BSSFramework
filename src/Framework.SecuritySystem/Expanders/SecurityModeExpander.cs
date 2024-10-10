using Framework.SecuritySystem.SecurityRuleInfo;

namespace Framework.SecuritySystem.Expanders;

public class SecurityModeExpander(IEnumerable<DomainModeSecurityRuleInfo> infoList) : ISecurityModeExpander
{
    private readonly IReadOnlyDictionary<DomainSecurityRule.DomainModeSecurityRule, DomainSecurityRule> dict =
        infoList.Select(info => (info.SecurityRule, info.Implementation)).ToDictionary();

    public DomainSecurityRule? TryExpand(DomainSecurityRule.DomainModeSecurityRule securityRule)
    {
        return this.dict.GetValueOrDefault(securityRule);
    }
}
