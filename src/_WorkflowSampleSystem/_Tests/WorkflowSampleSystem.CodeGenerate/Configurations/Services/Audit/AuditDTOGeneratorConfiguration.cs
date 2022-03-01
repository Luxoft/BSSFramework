using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.Audit;

using TypeExtensions = Framework.Projection.TypeExtensions;

namespace WorkflowSampleSystem.CodeGenerate.Configurations.Services.Audit
{
    public class AuditDTOGeneratorConfiguration : AuditDTOGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public AuditDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        protected override string DomainObjectPropertyRevisionsDTOPrefixName => "WorkflowSampleSystem";

        protected override string PropertyRevisionDTOPrefixName => "WorkflowSampleSystem";

        protected override IEnumerable<Type> GetDomainTypes()
        {
            return base.GetDomainTypes().Where(z => !TypeExtensions.IsProjection(z));
        }
    }
}
