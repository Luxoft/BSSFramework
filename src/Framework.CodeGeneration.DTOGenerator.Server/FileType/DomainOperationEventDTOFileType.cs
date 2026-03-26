using Framework.BLL.Domain.Serialization;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.Events;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileType;

public record DomainOperationEventDTOFileType(EventOperation EventOperation) : DTOFileType("OperationEventDTO", DTORole.Event)
{
    public override string ToString()
    {
        return $"{this.Name} ({this.EventOperation.Name})";
    }
}
