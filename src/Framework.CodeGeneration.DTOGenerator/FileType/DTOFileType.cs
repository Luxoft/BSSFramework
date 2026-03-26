using Framework.BLL.Domain.Serialization;

namespace Framework.CodeGeneration.DTOGenerator.FileType;

public record DTOFileType(string Name, DTORole Role) : RoleFileType(Name, Role);
