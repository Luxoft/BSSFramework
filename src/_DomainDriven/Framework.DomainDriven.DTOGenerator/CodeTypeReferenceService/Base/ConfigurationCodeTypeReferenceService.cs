using System;
using System.CodeDom;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator;

public class ConfigurationCodeTypeReferenceService<TConfiguration> : CodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ConfigurationCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public override CodeTypeReference GetCodeTypeReferenceByType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.ToTypeReference();
    }
}
