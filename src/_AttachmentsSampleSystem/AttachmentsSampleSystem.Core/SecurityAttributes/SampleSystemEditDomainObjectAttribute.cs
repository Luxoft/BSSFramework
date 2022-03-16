using System;
using Framework.Security;

namespace AttachmentsSampleSystem
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class AttachmentsSampleSystemEditDomainObjectAttribute : EditDomainObjectAttribute
    {
        public AttachmentsSampleSystemEditDomainObjectAttribute(AttachmentsSampleSystemSecurityOperationCode securityOperation)
            : base(securityOperation)
        {
        }

        public AttachmentsSampleSystemEditDomainObjectAttribute(Type editSecurityType)
            : base(editSecurityType)
        {
        }
    }
}
