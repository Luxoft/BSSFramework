using System;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Событие над объектом (Save, Remove etc), сохраняемое в базу.
/// </summary>
[BLLRole]
[NotAuditedClass]
public class DomainObjectEvent : AuditPersistentDomainObjectBase
{
    private readonly QueueProgressStatus status;

    private readonly string hostName;

    private readonly DateTime? processDate;

    private DomainTypeEventOperation operation;

    private Guid domainObjectId;

    private long revision;

    private string serializeData;

    private string serializeType;

    private string queueTag;

    private int size;

    ////private readonly long number;

    public virtual DomainTypeEventOperation Operation
    {
        get { return this.operation; }
        set { this.operation = value; }
    }

    public virtual Guid DomainObjectId
    {
        get { return this.domainObjectId; }
        set { this.domainObjectId = value; }
    }

    public virtual long Revision
    {
        get { return this.revision; }
        set { this.revision = value; }
    }

    [MaxLength]
    public virtual string SerializeData
    {
        get { return this.serializeData; }
        set { this.serializeData = value; }
    }

    [MaxLength]
    public virtual string SerializeType
    {
        get { return this.serializeType; }
        set { this.serializeType = value; }
    }

    [Required]
    public virtual string QueueTag
    {
        get { return this.queueTag; }
        set { this.queueTag = value; }
    }

    public virtual QueueProgressStatus Status => this.status;

    public virtual string HostName => this.hostName;

    public virtual DateTime? ProcessDate => this.processDate;

    public virtual int Size
    {
        get { return this.size; }
        set { this.size = value; }
    }

    ////[MappingProperty(CanInsert = false, CanUpdate = false)]
    ////public virtual long Number
    ////{
    ////    get { return this.number; }
    ////}
}
