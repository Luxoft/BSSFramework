using System;

using Framework.Core;

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
            : this(default(Enum))
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции
    /// </summary>
    /// <param name="securityOperationCode">Операция просмотра</param>
    public EditDomainObjectAttribute(Enum securityOperationCode)
            : base(securityOperationCode)
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции из Edit-атрибута типа
    /// </summary>
    /// <param name="editSecurityType">Доменный тип</param>
    public EditDomainObjectAttribute(Type editSecurityType)
            : this(editSecurityType.FromMaybe(() => new NullReferenceException("editSecurityType")).GetEditDomainObjectCode())
    {
    }
}
