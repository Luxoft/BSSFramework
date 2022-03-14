using System;

using Framework.DomainDriven.BLLCoreGenerator;

namespace Framework.Attachments.TestGenerate
{
    public partial class BLLCoreGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
