using System;

using Framework.Attachments.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

namespace Framework.Attachments.BLL
{
    public partial class AttachmentContainerBLLFactory : BLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IAttachmentBLLContextModule contextModule;

        public AttachmentContainerBLLFactory(IAttachmentBLLContextModule contextModule)
            : base (contextModule.Context)
        {
            this.contextModule = contextModule;
        }

        public IAttachmentContainerBLL Create(DomainType domainType, BLLSecurityMode securityMode)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            var targetSystem = this.contextModule.GetPersistentTargetSystemService(domainType.TargetSystem);

            var provider = targetSystem.GetAttachmentSecurityProvider<AttachmentContainer>(c => c, domainType, securityMode);

            return new AttachmentContainerBLL(this.contextModule, provider);
        }
    }
}
