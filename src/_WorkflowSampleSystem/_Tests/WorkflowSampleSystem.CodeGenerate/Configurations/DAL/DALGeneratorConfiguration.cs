using System;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class DALGeneratorConfiguration : Framework.DomainDriven.NHibernate.DALGenerator.GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public DALGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }
    }
}
