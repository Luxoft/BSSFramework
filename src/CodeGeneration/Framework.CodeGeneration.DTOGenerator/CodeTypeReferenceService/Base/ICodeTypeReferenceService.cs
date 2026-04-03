using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public interface ICodeTypeReferenceService
{
    CodeTypeReference GetCodeTypeReferenceByType(Type type);

    RoleFileType GetFileType(PropertyInfo property);
}
