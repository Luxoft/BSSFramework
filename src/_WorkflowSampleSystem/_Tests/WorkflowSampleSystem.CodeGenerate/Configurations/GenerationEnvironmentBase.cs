using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;

using WorkflowSampleSystem.CustomReports.Employee;
using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.CodeGenerate
{
    public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
    {
        public readonly IProjectionEnvironment MainProjectionEnvironment;

        public readonly IProjectionEnvironment LegacyProjectionEnvironment;

        protected GenerationEnvironmentBase()
            : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
        {
            this.MainProjectionEnvironment = this.CreateDefaultProjectionLambdaEnvironment(new WorkflowSampleSystemProjectionSource());

            this.LegacyProjectionEnvironment = this.CreateDefaultProjectionLambdaEnvironment(
                new LegacyWorkflowSampleSystemProjectionSource(),
                this.GetCreateProjectionLambdaSetupParams(projectionSubNamespace: "LegacyProjections", useDependencySecurity: false));
        }


        public override Type SecurityOperationCodeType { get; } = typeof(WorkflowSampleSystemSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(WorkflowSampleSystemOperationContext);


        protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
        {
            return base.GetDomainObjectAssemblies().Concat(new[] { typeof(EmployeeReport).Assembly });
        }

        protected override IEnumerable<IProjectionEnvironment> GetProjectionEnvironments()
        {
            yield return this.MainProjectionEnvironment;

            yield return this.LegacyProjectionEnvironment;

            yield return this.CreateManualProjectionLambdaEnvironment(typeof(WorkflowSampleSystem.Domain.ManualProjections.TestManualEmployeeProjection).Assembly);
        }
    }
}
