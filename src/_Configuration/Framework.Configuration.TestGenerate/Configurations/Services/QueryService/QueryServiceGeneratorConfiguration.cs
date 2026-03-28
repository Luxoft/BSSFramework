using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Query;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Configuration.TestGenerate;

public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }
}
