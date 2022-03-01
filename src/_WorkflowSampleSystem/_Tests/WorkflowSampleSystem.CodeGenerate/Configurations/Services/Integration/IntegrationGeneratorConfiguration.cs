using System;

using Framework.DomainDriven.ServiceModelGenerator;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class IntegrationGeneratorConfiguration : IntegrationGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public IntegrationGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}