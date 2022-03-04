using System;

using Framework.Security;

namespace WorkflowSampleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class WorkflowSampleSystemApproveOperationAttribute : ApproveOperationAttribute
{
    public WorkflowSampleSystemApproveOperationAttribute(WorkflowSampleSystemSecurityOperationCode operation)
            : base(operation)
    {
    }
}
