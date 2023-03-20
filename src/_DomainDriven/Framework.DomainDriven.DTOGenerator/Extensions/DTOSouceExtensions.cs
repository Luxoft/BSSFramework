using System;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator;

public static class DTOSouceExtensions
{
    public static bool IsPersistent<TConfiguration>(this IDTOSource<TConfiguration> source)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Configuration.IsPersistentObject(source.DomainType);
    }
}
