using Framework.DomainDriven.ServiceModelGenerator;

namespace SampleSystem.CodeGenerate;

public class IntegrationGeneratorConfiguration : IntegrationGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public IntegrationGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }
}
