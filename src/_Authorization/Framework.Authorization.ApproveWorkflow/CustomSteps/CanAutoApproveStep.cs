using System;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.DomainDriven;

using JetBrains.Annotations;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class CanAutoApproveStep : IStepBody
{
    private readonly IAuthorizationBLLContext context;

    private readonly IWorkflowApproveProcessor workflowApproveProcessor;

    private readonly IDBSession dbSession;

    public CanAutoApproveStep([NotNull] IAuthorizationBLLContext context,
                              [NotNull] IWorkflowApproveProcessor workflowApproveProcessor,
                              IDBSession dbSession)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.workflowApproveProcessor = workflowApproveProcessor ?? throw new ArgumentNullException(nameof(workflowApproveProcessor));

        this.dbSession = dbSession;
        this.dbSession.AsReadOnly();
    }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var workflowObject = (ApproveOperationWorkflowObject)context.Workflow.Data;

        workflowObject.AutoApprove = this.CanAutoApprove(workflowObject);

        return ExecutionResult.Next();
    }

    private bool CanAutoApprove(ApproveOperationWorkflowObject workflowObject)
    {
        var permission = this.context.Logics.Permission.GetById(workflowObject.PermissionId, true);

        var operation = this.context.Logics.Operation.GetById(workflowObject.OperationId, true);

        return this.workflowApproveProcessor.CanAutoApprove(permission, operation);
    }
}
