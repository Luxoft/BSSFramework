using System.Reflection;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class SourceTypeClientSecurityRuleInfoSource(IClientSecurityRuleNameExtractor clientSecurityRuleNameExtractor, Type sourceType) : IClientSecurityRuleInfoSource
{
    private readonly ClientSecurityRuleInfo[] infos =

        sourceType.GetProperties(BindingFlags.Static | BindingFlags.Public)
                  .Where(prop => typeof(DomainSecurityRule).IsAssignableFrom(prop.PropertyType))
                  .Select(prop => new ClientSecurityRuleInfo(clientSecurityRuleNameExtractor.ExtractName(prop), (DomainSecurityRule)prop.GetValue(null)!))
                  .ToArray();

    public IEnumerable<ClientSecurityRuleInfo> GetInfos() => this.infos;
}
