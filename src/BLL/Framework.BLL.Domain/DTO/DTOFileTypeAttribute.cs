using Framework.BLL.Domain.Serialization;

namespace Framework.BLL.Domain.DTO;

/// <summary>
/// Атрибут сгенерированного типа
/// </summary>
public class DTOFileTypeAttribute(Type domainType, string name, DTORole role) : Attribute
{
    /// <summary>
    /// Доменный тип
    /// </summary>
    public Type DomainType { get; } = domainType;

    /// <summary>
    /// Генерируемый тип
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Роль
    /// </summary>
    public DTORole Role { get; } = role;

    /// <summary>
    /// Дополнительные данные генерации
    /// </summary>
    public string ExternalData { get; init; }
}
