using System;

using Framework.DomainDriven.Serialization;

using JetBrains.Annotations;

namespace Framework.DomainDriven;

/// <summary>
/// Атрибут сгенерированного типа
/// </summary>
public class DTOFileTypeAttribute : Attribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="domainType">Доменный тип</param>
    /// <param name="name">Генерируемый тип</param>
    /// <param name="role">Роль</param>
    public DTOFileTypeAttribute([NotNull] Type domainType, [NotNull] string name, DTORole role)
    {
        this.DomainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.Role = role;
    }

    /// <summary>
    /// Доменный тип
    /// </summary>
    public Type DomainType { get; }

    /// <summary>
    /// Генерируемый тип
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Роль
    /// </summary>
    public DTORole Role { get; }

    /// <summary>
    /// Дополнительные данные генерации
    /// </summary>
    public string ExternalData { get; set; }
}
