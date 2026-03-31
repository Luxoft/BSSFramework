using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace SampleSystem.CodeGenerate;

public static class GeneratorConfigurationExtensions
{
    public static IEnumerable<PropertyInfo> GetFullRefDTOProperties<TConfiguration>(this TConfiguration configuration, Type domainType)
            where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetDomainTypeProperties(domainType, BaseFileType.FullDTO);
    }


    public static IEnumerable<PropertyInfo> GetSimpleRefFullDetailDTOProperties<TConfiguration>(this TConfiguration configuration, Type domainType)
            where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetDomainTypeProperties(domainType, BaseFileType.RichDTO);
    }
}
