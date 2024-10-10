namespace Framework.SecuritySystem.SecurityRuleInfo;

public class DomainModeClientSecurityRuleInfoSource(IClientSecurityRuleNameExtractor clientSecurityRuleNameExtractor, IClientDomainModeSecurityRuleSource clientDomainModeSecurityRuleSource)
    : IClientSecurityRuleInfoSource
{
    private readonly ClientSecurityRuleInfo[] infos =

        clientDomainModeSecurityRuleSource.GetRules()
            .Select(securityRule => new ClientSecurityRuleInfo(clientSecurityRuleNameExtractor.ExtractName(securityRule), securityRule))
            .ToArray();

    public IEnumerable<ClientSecurityRuleInfo> GetInfos() => this.infos;
}
