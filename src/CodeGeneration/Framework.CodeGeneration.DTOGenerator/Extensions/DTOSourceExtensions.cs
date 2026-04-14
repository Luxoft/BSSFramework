using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class DTOSourceExtensions
{
    public static bool IsPersistent<TConfiguration>(this IDTOSource<TConfiguration> source)
            where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Configuration.IsPersistentObject(source.DomainType);
    }
}
