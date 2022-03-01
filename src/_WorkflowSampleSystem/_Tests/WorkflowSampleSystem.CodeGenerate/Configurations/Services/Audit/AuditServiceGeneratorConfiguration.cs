using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.ServiceModelGenerator;

using TypeExtensions = Framework.Projection.TypeExtensions;

namespace WorkflowSampleSystem.CodeGenerate.Configurations.Services.Audit
{
    public class AuditServiceGeneratorConfiguration : AuditGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public AuditServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string ServiceContractNamespace => "http://WorkflowSampleSystem.luxoft.com/AuditFacade";

        protected override IEnumerable<Type> GetDomainTypes()
        {
            return base.GetDomainTypes().Where(z => !TypeExtensions.IsProjection(z));
        }
    }
}
