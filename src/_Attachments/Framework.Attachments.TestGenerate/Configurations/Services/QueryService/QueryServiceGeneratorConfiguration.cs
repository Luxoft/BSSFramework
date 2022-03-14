using System;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Attachments.TestGenerate
{
    public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}