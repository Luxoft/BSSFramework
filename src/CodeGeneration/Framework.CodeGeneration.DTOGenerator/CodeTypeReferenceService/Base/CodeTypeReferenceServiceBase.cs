using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public abstract class CodeTypeReferenceService<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), ICodeTypeReferenceService
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    public virtual CodeTypeReference GetCodeTypeReferenceByType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (type.IsArray)
        {
            return new CodeTypeReference(this.GetCodeTypeReferenceByType(type.GetElementType()), type.GetArrayRank());
        }
        else
        {
            return this.Configuration.DefaultCodeTypeReferenceService.GetCodeTypeReferenceByType(type);
        }
    }

    public virtual RoleFileType GetFileType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return null;
    }
}
