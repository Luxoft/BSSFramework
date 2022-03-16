using System;

namespace AttachmentsSampleSystem.CodeGenerate
{
    public class BLLGeneratorConfiguration : Framework.DomainDriven.BLLGenerator.GeneratorConfigurationBase<
            ServerGenerationEnvironment>
    {
        public BLLGeneratorConfiguration(ServerGenerationEnvironment environment)
                : base(environment)
        {
        }
    }
}
