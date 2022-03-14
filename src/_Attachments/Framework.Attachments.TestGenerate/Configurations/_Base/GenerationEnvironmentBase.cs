using System;

using Framework.Configuration;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Attachments.TestGenerate
{
    public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
    {
        public readonly string DTODataContractNamespace = "Configuration";


        protected GenerationEnvironmentBase()
                : base(v => v.Id)
        {
        }

        public override Type SecurityOperationCodeType { get; } = typeof(ConfigurationSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(ConfigurationOperationContext);
    }
}
