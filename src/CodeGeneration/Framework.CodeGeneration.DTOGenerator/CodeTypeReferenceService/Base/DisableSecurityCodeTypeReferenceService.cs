using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public class DisableSecurityCodeTypeReferenceService<TConfiguration>(TConfiguration configuration) : PropertyCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override bool IsOptional(PropertyInfo property)
    {
        return false;
    }
}
