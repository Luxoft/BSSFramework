using Framework.Restriction;
using Framework.Validation.Attributes;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Модель для форсирования ручного вызова евента по объекту
/// </summary>
public class DomainTypeEventModel : DomainObjectBase
{
    /// <summary>
    /// Вызываемая операция
    /// </summary>
    [Required]
    public DomainTypeEventOperation Operation { get; set; } = null!;

    /// <summary>
    /// Идент объекта
    /// </summary>
    [Required]
    [AnyElementsValidator]
    public List<Guid> DomainObjectIdents { get; set; } = [];

    /// <summary>
    /// Ревизия
    /// </summary>
    public long? Revision { get; set; }
}
