using Framework.DomainDriven.BLLGenerator;

namespace Framework.Workflow.TestGenerate
{
    public class BLLGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public BLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
