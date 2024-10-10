using Framework.SecuritySystem.SecurityRuleInfo;

namespace Framework.SecuritySystem;

public class ClientSecurityRuleParser(IClientSecurityRuleInfoSource clientSecurityRuleInfoSource) : ISecurityRuleParser
{
    private readonly IReadOnlyDictionary<string, DomainSecurityRule> dict = clientSecurityRuleInfoSource.GetInfos()
        .ToDictionary(info => info.Header.Name, info => info.Implementation);

    public virtual SecurityRule Parse<TDomainObject>(string name)
    {
        return this.dict.GetValueOrDefault(name) ?? throw new Exception($"ClientRule with name \"{name}\" not found");
    }
}
