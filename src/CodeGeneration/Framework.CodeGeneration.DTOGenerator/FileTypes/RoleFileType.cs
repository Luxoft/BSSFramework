using Framework.BLL.Domain.Serialization;

namespace Framework.CodeGeneration.DTOGenerator.FileTypes;

public record RoleFileType(string Name, DTORole Role) : BaseFileType(Name);
