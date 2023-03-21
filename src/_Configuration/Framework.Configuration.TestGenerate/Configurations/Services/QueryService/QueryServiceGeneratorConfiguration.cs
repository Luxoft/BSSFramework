using System;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Configuration.TestGenerate;

public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }
}
