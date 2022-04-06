using System;

namespace Framework.Authorization.ApproveWorkflow;
public class ApproveOperationWorkflowObject
{
    public bool AutoApprove { get; set; }

    public Guid OperationId { get; set; }

    public Guid PermissionId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public ApproveOperationWorkflowStatus Status { get; set; }
}
