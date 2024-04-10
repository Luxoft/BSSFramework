using Framework.SecuritySystem;

namespace Framework.Security;

/// <summary>
/// Атрибут для редактирования объекта (или его свойства)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
public class EditDomainObjectAttribute : DomainObjectAccessAttribute
{
    /// <summary>
    /// Пустой констуктор для кастомной безопасности
    /// </summary>
    public EditDomainObjectAttribute()
        : base(null)
    {
    }

    public EditDomainObjectAttribute(SecurityRule securityRule)
        : base(securityRule)
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции
    /// </summary>
    public EditDomainObjectAttribute(Type securityRuleType, string name)
        : base(securityRuleType, name)
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции из Edit-атрибута типа
    /// </summary>
    /// <param name="editSecurityType">Доменный тип</param>
    public EditDomainObjectAttribute(Type editSecurityType)
        : base(editSecurityType.GetEditSecurityRule(true))
    {
    }
}
