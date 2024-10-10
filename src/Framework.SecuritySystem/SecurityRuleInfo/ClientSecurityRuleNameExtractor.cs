using System.Reflection;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class ClientSecurityRuleNameExtractor : IClientSecurityRuleNameExtractor
{
    public string ExtractName(PropertyInfo propertyInfo) => propertyInfo.Name;

    public string ExtractName(DomainSecurityRule.DomainModeSecurityRule securityRule) => $"{securityRule.DomainType.Name}{securityRule.Mode.Name}";
}
