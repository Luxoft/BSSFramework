using Framework.DomainDriven.ServiceModelGenerator;

namespace AttachmentsSampleSystem.CodeGenerate
{
    public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
