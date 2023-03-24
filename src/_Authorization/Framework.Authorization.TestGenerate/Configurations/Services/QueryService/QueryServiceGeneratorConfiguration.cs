using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Authorization.TestGenerate;

public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }
}
