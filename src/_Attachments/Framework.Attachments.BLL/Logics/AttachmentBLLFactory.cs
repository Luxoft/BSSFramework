using System;

using Framework.Configuration.Domain;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL
{
    public class AttachmentBLLFactory
    {
        [NotNull]
        private readonly AttachmentBLLContextModule contextModule;

        public AttachmentBLLFactory([NotNull] AttachmentBLLContextModule contextModule)
        {
            this.contextModule = contextModule ?? throw new ArgumentNullException(nameof(contextModule));
        }

        public IAttachmentBLL Create(DomainType domainType, BLLSecurityMode securityMode)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            var targetSystem = this.contextModule.GetPersistentTargetSystemService(domainType.TargetSystem);

            var provider = targetSystem.GetAttachmentSecurityProvider(domainType, securityMode);

            return new AttachmentBLL(this.contextModule, provider);
        }
    }
}
