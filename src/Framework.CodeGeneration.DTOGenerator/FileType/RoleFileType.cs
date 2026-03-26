using Framework.BLL.Domain.Serialization;

namespace Framework.CodeGeneration.DTOGenerator.FileType;

public record RoleFileType(string Name, DTORole Role) : BaseFileType(Name);
