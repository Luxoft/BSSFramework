using Framework.Database.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Unique number of the system element
/// </summary>
[UniqueGroup]
[NotAuditedClass]
public class Sequence : BaseDirectory
{
    private long number;

    /// <summary>
    /// Element value
    /// </summary>
    public virtual long Number
    {
        get => this.number;
        set => this.number = value;
    }
}
