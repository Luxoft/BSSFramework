using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Validation;

namespace Framework.Configuration.Domain;

/// <summary>
/// Уникальный номер элемента системы
/// </summary>
[UniqueGroup]
[NotAuditedClass]
[IgnoreHbmMapping]
public class Sequence : BaseDirectory, INumberObject<long>
{
    private long number;

    /// <summary>
    /// Значение элемента
    /// </summary>
    [Int64ValueValidator(Min = 0)]
    public virtual long Number
    {
        get { return this.number; }
        set { this.number = value; }
    }
}
