using System;
using System.Linq;

using Framework.Security;

namespace AttachmentsSampleSystem
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class AttachmentsSampleSystemViewDomainObjectAttribute : ViewDomainObjectAttribute
    {
        public AttachmentsSampleSystemViewDomainObjectAttribute(Type viewSecurityType)
            : base(viewSecurityType)
        {
        }

        public AttachmentsSampleSystemViewDomainObjectAttribute(AttachmentsSampleSystemSecurityOperationCode primaryOperation, params AttachmentsSampleSystemSecurityOperationCode[] secondaryOperations)
            : base(primaryOperation, secondaryOperations.Cast<Enum>())
        {
        }
    }
}