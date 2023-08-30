using Framework.Security;

namespace Framework.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class ConfigurationEditDomainObjectAttribute : EditDomainObjectAttribute
{
    public ConfigurationEditDomainObjectAttribute(ConfigurationSecurityOperationCode securityOperation)
            : base(securityOperation)
    {

    }
}
