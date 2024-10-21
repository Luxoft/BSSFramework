using Framework.SecuritySystem;

namespace Framework.Security;

/// <summary>
/// Атрибут для редактирования объекта (или его свойства)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class EditDomainObjectAttribute : DomainObjectAccessAttribute
{
    public EditDomainObjectAttribute(Type securityRuleType, string name)
        : base(securityRuleType, name)
    {
    }

    public EditDomainObjectAttribute(Type domainType, bool isEdit = true)
        : base(domainType, isEdit)
    {
    }

    public EditDomainObjectAttribute(SecurityRule securityRule)
        : base(securityRule)
    {
    }
}
