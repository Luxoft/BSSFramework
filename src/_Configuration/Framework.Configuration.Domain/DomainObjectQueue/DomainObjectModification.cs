using Framework.Application.Domain;
using Framework.Database;
using Framework.Database.Attributes;
using Framework.Database.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Модификация объекта хранимая в бд
/// </summary>
[UniqueGroup]
[NotAuditedClass]
public class DomainObjectModification : AuditPersistentDomainObjectBase, IVersionObject<long>
{
    private Guid domainObjectId;

    private long revision;

    private DomainType domainType = null!;

    private ModificationType type;

    private bool processed;

    private long version;

    /// <summary>
    /// Доменный тип
    /// </summary>
    [Required]
    [UniqueElement]
    public virtual DomainType DomainType
    {
        get => this.domainType;
        set => this.domainType = value;
    }

    [Required]
    [UniqueElement]
    public virtual Guid DomainObjectId
    {
        get => this.domainObjectId;
        set => this.domainObjectId = value;
    }

    [Required]
    [UniqueElement]
    public virtual long Revision
    {
        get => this.revision;
        set => this.revision = value;
    }

    public virtual ModificationType Type
    {
        get => this.type;
        set => this.type = value;
    }

    public virtual bool Processed
    {
        get => this.processed;
        set => this.processed = value;
    }

    [Version]
    public virtual long Version
    {
        get => this.version;
        set => this.version = value;
    }
}
