using Framework.DomainDriven.BLLGenerator;

namespace Framework.Configuration.TestGenerate;

public class BLLGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public BLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }
}
