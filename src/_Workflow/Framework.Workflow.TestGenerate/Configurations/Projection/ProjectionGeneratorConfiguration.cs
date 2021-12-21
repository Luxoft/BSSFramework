using System;

namespace Framework.Workflow.TestGenerate
{
    public class ProjectionGeneratorConfiguration : Framework.DomainDriven.ProjectionGenerator.GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public ProjectionGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
