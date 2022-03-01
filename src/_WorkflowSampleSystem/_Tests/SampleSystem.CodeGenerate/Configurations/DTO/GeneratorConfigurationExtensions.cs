using System;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator;

namespace SampleSystem.CodeGenerate
{
    public static class GeneratorConfigurationExtensions
    {
        public static IEnumerable<PropertyInfo> GetFullRefDTOProperties<TConfiguration>(this TConfiguration configuration, Type domainType)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration.GetDomainTypeProperties(domainType, FileType.FullDTO);
        }


        public static IEnumerable<PropertyInfo> GetSimpleRefFullDetailDTOProperties<TConfiguration>(this TConfiguration configuration, Type domainType)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration.GetDomainTypeProperties(domainType, FileType.RichDTO);
        }
    }
}