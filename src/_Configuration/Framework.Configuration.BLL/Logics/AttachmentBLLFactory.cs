using System;

using Framework.Configuration.Domain;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL
{
    public partial class AttachmentBLLFactory
    {
        public IAttachmentBLL Create(DomainType domainType, BLLSecurityMode securityMode)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            var targetSystem = this.Context.GetPersistentTargetSystemService(domainType.TargetSystem);

            var provider = targetSystem.GetAttachmentSecurityProvider(domainType, securityMode);

            return this.Create(provider);
        }
    }
}
