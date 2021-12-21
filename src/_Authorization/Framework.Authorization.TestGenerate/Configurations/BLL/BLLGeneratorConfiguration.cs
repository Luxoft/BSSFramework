using Framework.DomainDriven.BLLGenerator;

namespace Framework.Authorization.TestGenerate
{
    public class BLLGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public BLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
