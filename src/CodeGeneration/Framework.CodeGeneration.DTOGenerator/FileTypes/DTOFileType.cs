using Framework.BLL.Domain.Serialization;

namespace Framework.CodeGeneration.DTOGenerator.FileTypes;

public record DTOFileType(string Name, DTORole Role) : RoleFileType(Name, Role);
