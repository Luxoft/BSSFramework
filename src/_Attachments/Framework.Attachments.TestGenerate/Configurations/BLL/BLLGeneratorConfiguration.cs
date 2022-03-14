using Framework.DomainDriven.BLLGenerator;

namespace Framework.Attachments.TestGenerate
{
    public class BLLGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public BLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}