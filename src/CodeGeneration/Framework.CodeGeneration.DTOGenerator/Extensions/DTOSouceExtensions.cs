using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class DTOSouceExtensions
{
    public static bool IsPersistent<TConfiguration>(this IDTOSource<TConfiguration> source)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Configuration.IsPersistentObject(source.DomainType);
    }
}
