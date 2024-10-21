using Framework.SecuritySystem;

namespace Framework.Security;

/// <summary>
/// Атрибут для отображения объекта (или его свойства)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Property)]
public class ViewDomainObjectAttribute : DomainObjectAccessAttribute
{
    public ViewDomainObjectAttribute(Type securityRuleType, string name)
        : base(securityRuleType, name)
    {
    }

    public ViewDomainObjectAttribute(Type domainType, bool isEdit = false)
        : base(domainType, isEdit)
    {
    }

    public ViewDomainObjectAttribute(SecurityRule securityRule)
        : base(securityRule)
    {
    }
}
