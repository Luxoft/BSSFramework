using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public class ConfigurationCodeTypeReferenceService<TConfiguration>(TConfiguration configuration) : CodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override CodeTypeReference GetCodeTypeReferenceByType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.ToTypeReference();
    }
}
