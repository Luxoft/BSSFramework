using Framework.Core;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.Expanders;

public class SecurityRuleHeaderExpander : ISecurityRuleHeaderExpander
{
    private readonly IReadOnlyDictionary<DomainSecurityRule.SecurityRuleHeader, DomainSecurityRule> dict;

    public SecurityRuleHeaderExpander(IEnumerable<SecurityRuleHeaderInfo> securityRuleFullInfoList)
    {
        this.dict = securityRuleFullInfoList.ToDictionary(pair => pair.Header, pair => pair.Implementation);
    }

    public DomainSecurityRule Expand(DomainSecurityRule.SecurityRuleHeader securityRuleHeader)
    {
        return this.dict.GetValue(
            securityRuleHeader,
            () => new Exception($"Implementation for {nameof(SecurityRule)} \"{securityRuleHeader}\" not found"));
    }
}
