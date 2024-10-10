using System.Reflection;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public interface IClientSecurityRuleNameExtractor
{
    string ExtractName(PropertyInfo propertyInfo);

    string ExtractName(DomainSecurityRule.DomainModeSecurityRule securityRule);
}
