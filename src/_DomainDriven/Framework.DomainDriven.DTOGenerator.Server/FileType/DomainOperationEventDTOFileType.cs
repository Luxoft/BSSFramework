using Framework.DomainDriven.Serialization;
using Framework.Events;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DomainOperationEventDTOFileType : DTOFileType, IEquatable<DomainOperationEventDTOFileType>
{
    public readonly EventOperation EventOperation;


    public DomainOperationEventDTOFileType(EventOperation domainObjectEvent)
        : base("OperationEventDTO", DTORole.Event)
    {
        this.EventOperation = domainObjectEvent ?? throw new ArgumentNullException(nameof(domainObjectEvent));
    }


    public override bool Equals(FileType other)
    {
        return this.Equals(other as DomainOperationEventDTOFileType);
    }

    public virtual bool Equals(DomainOperationEventDTOFileType other)
    {
        return base.Equals(other) && this.EventOperation == other.EventOperation;
    }

    public override string ToString()
    {
        return $"{this.Name} ({this.EventOperation.Name})";
    }
}
