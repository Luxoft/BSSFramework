namespace Framework.Authorization.ApproveWorkflow;
public class ApproveOperationWorkflowObject
{
    public string WorkflowInstanceId { get; set; }

    public bool AutoApprove { get; set; }

    public Guid OperationId { get; set; }

    public Guid PermissionId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public ApproveOperationWorkflowStatus Status { get; set; }



    public string ApprovedByUser { get; set; }

    public string ApproveEventId { get; set; }



    public string RejectedByUser { get; set; }

    public string RejectEventId { get; set; }
}
