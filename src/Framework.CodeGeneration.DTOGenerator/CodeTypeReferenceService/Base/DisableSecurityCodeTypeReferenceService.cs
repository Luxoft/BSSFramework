using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public class DisableSecurityCodeTypeReferenceService<TConfiguration> : PropertyCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public DisableSecurityCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public override bool IsOptional(PropertyInfo property)
    {
        return false;
    }
}
