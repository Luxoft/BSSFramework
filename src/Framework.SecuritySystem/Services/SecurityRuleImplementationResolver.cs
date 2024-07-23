using Framework.Core;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleImplementationResolver : ISecurityRuleImplementationResolver
{
    private readonly IReadOnlyDictionary<DomainSecurityRule.SecurityRuleHeader, DomainSecurityRule> dict;

    public SecurityRuleImplementationResolver(IEnumerable<SecurityRuleFullInfo> securityRuleFullInfoList)
    {
        this.dict = securityRuleFullInfoList.ToDictionary(pair => pair.Header, pair => pair.Implementation);
    }

    public DomainSecurityRule Resolve(DomainSecurityRule.SecurityRuleHeader securityRuleHeader)
    {
        return this.dict.GetValue(
            securityRuleHeader,
            () => new Exception($"Implementation for {nameof(SecurityRule)} \"{securityRuleHeader}\" not found"));
    }
}
