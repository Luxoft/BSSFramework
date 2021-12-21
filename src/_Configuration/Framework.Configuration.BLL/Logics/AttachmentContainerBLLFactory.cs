using System;

using Framework.Configuration.Domain;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL
{
    public partial class AttachmentContainerBLLFactory
    {
        public IAttachmentContainerBLL Create(DomainType domainType, BLLSecurityMode securityMode)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            var targetSystem = this.Context.GetPersistentTargetSystemService(domainType.TargetSystem);

            var provider = targetSystem.GetAttachmentSecurityProvider<AttachmentContainer>(c => c, domainType, securityMode);

            return this.Create(provider);
        }
    }
}
