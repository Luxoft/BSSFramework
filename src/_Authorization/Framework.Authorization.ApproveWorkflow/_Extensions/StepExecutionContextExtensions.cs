using WorkflowCore.Interface;

namespace Framework.Authorization.ApproveWorkflow;

public static class StepExecutionContextExtensions
{
    public static Guid GetOperationId(this IStepExecutionContext stepExecutionContext)
    {
        return ((ApproveOperationWorkflowObject)stepExecutionContext.Item).OperationId;
    }

    public static ApproveOperationWorkflowObject GetOperation(this IStepExecutionContext stepExecutionContext)
    {
        return stepExecutionContext.GetPermission().Operations.Single(op => op.OperationId == stepExecutionContext.GetOperationId());
    }

    private static ApprovePermissionWorkflowObject GetPermission(this IStepExecutionContext stepExecutionContext)
    {
        return ((ApprovePermissionWorkflowObject)stepExecutionContext.Workflow.Data);
    }
}
