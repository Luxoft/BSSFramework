using Framework.Authorization.Domain;

namespace Framework.Authorization.ApproveWorkflow;

public interface IWorkflowApproveProcessor
{
    bool CanAutoApprove(Permission permission, Operation approveOperation);

    ApprovePermissionWorkflowObject GetPermissionStartupObject(Permission permission);
}
