using System;

using Framework.Attachments.Domain;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;

namespace Framework.Attachments.TestGenerate
{
    public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
    {
        public readonly string DTODataContractNamespace = "Attachments";


        protected GenerationEnvironmentBase()
                : base(v => v.Id)
        {
        }

        public sealed override Type SecurityOperationCodeType { get; } = typeof(AttachmentsSecurityOperationCode);

        public sealed override Type OperationContextType { get; } = typeof(AttachmentsOperationContext);
    }
}
