using Framework.DomainDriven.Serialization;
using Framework.Events;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DomainOperationEventDTOFileType : DTOFileType, IEquatable<DomainOperationEventDTOFileType>
{
    public readonly EventOperation EventOperation;


    public DomainOperationEventDTOFileType(EventOperation eventOperation)
        : base("OperationEventDTO", DTORole.Event)
    {
        this.EventOperation = eventOperation ?? throw new ArgumentNullException(nameof(eventOperation));
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
