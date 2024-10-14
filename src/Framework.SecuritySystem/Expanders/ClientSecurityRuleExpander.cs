using Framework.SecuritySystem.SecurityRuleInfo;

namespace Framework.SecuritySystem.Expanders;

public class ClientSecurityRuleExpander(IClientSecurityRuleInfoSource clientSecurityRuleInfoSource) : IClientSecurityRuleExpander
{
    private readonly IReadOnlyDictionary<DomainSecurityRule.ClientSecurityRule, DomainSecurityRule> dict = clientSecurityRuleInfoSource.GetInfos()
        .ToDictionary(info => info.Rule, info => info.Implementation);


    public DomainSecurityRule Expand(DomainSecurityRule.ClientSecurityRule securityRule)
    {
        return this.dict.GetValueOrDefault(securityRule)
               ?? throw new Exception($"{nameof(DomainSecurityRule.ClientSecurityRule)} with name \"{securityRule.Name}\" not found");
    }
}
