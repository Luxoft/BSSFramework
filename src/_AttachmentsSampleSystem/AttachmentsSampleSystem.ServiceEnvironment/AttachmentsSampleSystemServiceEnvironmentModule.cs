using Framework.Attachments.Environment;

using JetBrains.Annotations;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Domain;

namespace AttachmentsSampleSystem.ServiceEnvironment;

public class AttachmentsSampleSystemServiceEnvironmentModule : AttachmentsServiceEnvironmentModule<AttachmentsSampleSystemServiceEnvironment, AttachmentsSampleSystemBLLContextContainer, IAttachmentsSampleSystemBLLContext, PersistentDomainObjectBase>
{
    public AttachmentsSampleSystemServiceEnvironmentModule([NotNull] AttachmentsSampleSystemServiceEnvironment mainServiceEnvironment)
            : base(mainServiceEnvironment)
    {
    }
}
