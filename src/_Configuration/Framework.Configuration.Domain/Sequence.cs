using Framework.Database.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Уникальный номер элемента системы
/// </summary>
[UniqueGroup]
[NotAuditedClass]
public class Sequence : BaseDirectory
{
    private long number;

    /// <summary>
    /// Значение элемента
    /// </summary>
    public virtual long Number
    {
        get => this.number;
        set => this.number = value;
    }
}
