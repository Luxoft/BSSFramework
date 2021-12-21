using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModelGenerator;

namespace SampleSystem.CodeGenerate
{
    public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}