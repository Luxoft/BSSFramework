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

    public EditDomainObjectAttribute(SecurityOperation securityOperation)
        : base(securityOperation)
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции
    /// </summary>
    public EditDomainObjectAttribute(Type securityOperationType, string name)
        : base(securityOperationType, name)
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции из Edit-атрибута типа
    /// </summary>
    /// <param name="editSecurityType">Доменный тип</param>
    public EditDomainObjectAttribute(Type editSecurityType)
        : base(editSecurityType.GetEditSecurityOperation(true))
    {
    }
}
