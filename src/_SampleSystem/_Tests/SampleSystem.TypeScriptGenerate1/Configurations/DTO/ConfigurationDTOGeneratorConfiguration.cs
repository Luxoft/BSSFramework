using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using SampleSystem.TypeScriptGenerate.Configurations.Environments;

namespace SampleSystem.TypeScriptGenerate.Configurations.DTO
{
    public class ConfigurationDTOGeneratorConfiguration : TypeScriptDTOGeneratorConfiguration<ConfigurationGenerationEnvironment>
    {
        public ConfigurationDTOGeneratorConfiguration(ConfigurationGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
