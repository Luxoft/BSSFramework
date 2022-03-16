using System;

using Framework.Security;

namespace AttachmentsSampleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class AttachmentsSampleSystemApproveOperationAttribute : ApproveOperationAttribute
{
    public AttachmentsSampleSystemApproveOperationAttribute(AttachmentsSampleSystemSecurityOperationCode operation)
            : base(operation)
    {
    }
}
