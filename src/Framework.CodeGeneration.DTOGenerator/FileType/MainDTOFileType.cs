using Framework.BLL.Domain.Serialization;

namespace Framework.CodeGeneration.DTOGenerator.FileType;

public record MainDTOFileType(string Name, MainDTOFileType? BaseType, bool IsAbstract) : DTOFileType(Name, DTORole.Client);
