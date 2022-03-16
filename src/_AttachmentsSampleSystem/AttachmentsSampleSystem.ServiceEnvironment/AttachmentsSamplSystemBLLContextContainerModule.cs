using System;
using System.Collections.Generic;

using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Attachments.BLL;

using JetBrains.Annotations;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Domain;

namespace AttachmentsSampleSystem.ServiceEnvironment;

public class AttachmentsSamplSystemBLLContextContainerModule : AttachmentsBLLContextContainerModule<AttachmentsSampleSystemServiceEnvironment, AttachmentsSampleSystemBLLContextContainer, IAttachmentsSampleSystemBLLContext, PersistentDomainObjectBase, AttachmentsSampleSystemSecurityOperationCode>
{
    public AttachmentsSamplSystemBLLContextContainerModule([NotNull] AttachmentsSampleSystemServiceEnvironment mainServiceEnvironment, AttachmentsSampleSystemBLLContextContainer bllContextContainer)
            : base(mainServiceEnvironment.AttachmentsModule, mainServiceEnvironment, bllContextContainer)
    {
    }

    protected override IEnumerable<ITargetSystemService> GetAttachmentsTargetSystemServices()
    {
        yield return this.GetMainAttachmentsTargetSystemService();
    }
}
