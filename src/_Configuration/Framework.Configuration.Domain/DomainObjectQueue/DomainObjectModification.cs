using System;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Модификация объекта хранимая в бд
/// </summary>
[BLLRole]
[UniqueGroup]
[NotAuditedClass]
public class DomainObjectModification : AuditPersistentDomainObjectBase, ITypeObject<ModificationType>, IVersionObject<long>
{
    private Guid domainObjectId;

    private long revision;

    private DomainType domainType;

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
        get { return this.domainType; }
        set { this.domainType = value; }
    }

    [Required]
    [UniqueElement]
    public virtual Guid DomainObjectId
    {
        get { return this.domainObjectId; }
        set { this.domainObjectId = value; }
    }

    [Required]
    [UniqueElement]
    public virtual long Revision
    {
        get { return this.revision; }
        set { this.revision = value; }
    }

    public virtual ModificationType Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public virtual bool Processed
    {
        get { return this.processed; }
        set { this.processed = value; }
    }

    [Version]
    public virtual long Version
    {
        get { return this.version; }
        set { this.version = value; }
    }
}
