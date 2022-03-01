using Framework.DomainDriven.ServiceModelGenerator;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
