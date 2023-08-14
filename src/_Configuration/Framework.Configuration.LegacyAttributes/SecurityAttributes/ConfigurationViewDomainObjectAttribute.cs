using Framework.Security;

namespace Framework.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ConfigurationViewDomainObjectAttribute : ViewDomainObjectAttribute
{
    public ConfigurationViewDomainObjectAttribute(ConfigurationSecurityOperationCode securityOperation)
            : base(securityOperation)
    {

    }
}
