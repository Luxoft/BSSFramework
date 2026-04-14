using Framework.Database.Mapping;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Нотификация хранимая в бд
/// </summary>
[NotAuditedClass]
public class DomainObjectNotification : AuditPersistentDomainObjectBase
{
    private readonly QueueProgressStatus status;

    private readonly string hostName;

    private readonly DateTime? processDate;

    private string serializeData;

    private int size;

    [Required]
    [MaxLength]
    public virtual string SerializeData
    {
        get => this.serializeData;
        set => this.serializeData = value;
    }

    public virtual QueueProgressStatus Status => this.status;

    public virtual string HostName => this.hostName;

    public virtual DateTime? ProcessDate => this.processDate;

    public virtual int Size
    {
        get => this.size;
        set => this.size = value;
    }
}
