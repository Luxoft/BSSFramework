using Framework.Authorization.Domain;

namespace Framework.Authorization.ApproveWorkflow;

public class ApprovePermissionWorkflowObject
{
    public Guid PermissionId { get; set; }

    public string Name { get; set; }

    public PermissionStatus Status { get; set; }

    public List<ApproveOperationWorkflowObject> Operations { get; set; }

    public bool SomeOneOperationRejected { get; set; }

    public ApproveOperationWorkflowObject GetActualItem(ApproveOperationWorkflowObject unperObj)
    {
        return this.Operations.Single(wfObj => wfObj.OperationId == unperObj.OperationId);
    }

    public ApproveOperationWorkflowObject GetActualItem(object unperObj)
    {
        return this.GetActualItem((ApproveOperationWorkflowObject)unperObj);
    }
}
