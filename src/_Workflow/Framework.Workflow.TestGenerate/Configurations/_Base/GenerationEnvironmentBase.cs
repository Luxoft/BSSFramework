using System;

using Framework.Workflow.Domain;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;

namespace Framework.Workflow.TestGenerate
{
    public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
    {
        public readonly string DTODataContractNamespace = "Workflow";


        protected GenerationEnvironmentBase()
                : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
        {
        }

        public override Type SecurityOperationCodeType { get; } = typeof(WorkflowSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(WorkflowOperationContext);


        protected override IProjectionEnvironment GetProjectionEnvironment()
        {
            return this.CreateDefaultProjectionContractEnvironment();
        }
    }
}
