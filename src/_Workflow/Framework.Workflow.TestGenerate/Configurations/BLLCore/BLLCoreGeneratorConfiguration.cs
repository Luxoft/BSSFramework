using System;
using System.Linq;

using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.TestGenerate
{
    public partial class BLLCoreGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }


        public override Type FilterModelType { get; } = typeof(DomainObjectFilterModel<>);

        public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);

        public override Type CreateModelType { get; } = typeof(DomainObjectCreateModel<>);

        public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

        public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);

        public override bool GenerateDomainServiceConstructor(Type domainType)
        {
            return !new[] { typeof(WorkflowInstance), typeof(TaskInstance) }.Contains(domainType);
        }
    }
}
