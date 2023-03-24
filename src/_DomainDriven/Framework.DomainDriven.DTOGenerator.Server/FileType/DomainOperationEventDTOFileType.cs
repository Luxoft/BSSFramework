using System;

using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DomainOperationEventDTOFileType : DTOFileType, IEquatable<DomainOperationEventDTOFileType>
{
    public readonly Enum Operation;


    public DomainOperationEventDTOFileType(Enum operation)
            : base("OperationEventDTO", DTORole.Event)
    {
        this.Operation = operation ?? throw new ArgumentNullException(nameof(operation));
    }


    public override bool Equals(FileType other)
    {
        return this.Equals(other as DomainOperationEventDTOFileType);
    }

    public virtual bool Equals(DomainOperationEventDTOFileType other)
    {
        return base.Equals(other) && this.Operation.Equals(other.Operation);
    }

    public override string ToString()
    {
        return $"{this.Name} ({this.Operation})";
    }
}
