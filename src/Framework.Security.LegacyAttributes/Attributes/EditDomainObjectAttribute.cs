using Framework.SecuritySystem;

namespace Framework.Security;

/// <summary>
/// Атрибут для редактирования объекта (или его свойства)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class EditDomainObjectAttribute : DomainObjectAccessAttribute
{
    public EditDomainObjectAttribute(SecurityRule securityRule)
        : base(securityRule)
    {
    }

    public EditDomainObjectAttribute(Type domainType)
        : base(SecurityRule.Edit.ToDomain(domainType))
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции
    /// </summary>
    public EditDomainObjectAttribute(Type securityRuleType, string name)
        : base(securityRuleType, name)
    {
    }
}
