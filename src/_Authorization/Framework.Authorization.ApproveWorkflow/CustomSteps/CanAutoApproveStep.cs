using System;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class CanAutoApproveStep : IStepBody
{
    private readonly IWorkflowApproveProcessor workflowApproveProcessor;

    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

    public CanAutoApproveStep([NotNull] IWorkflowApproveProcessor workflowApproveProcessor, [NotNull] IContextEvaluator<IAuthorizationBLLContext> contextEvaluator)
    {
        this.workflowApproveProcessor = workflowApproveProcessor ?? throw new ArgumentNullException(nameof(workflowApproveProcessor));
        this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
    }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var workflowObject = (ApproveOperationWorkflowObject)context.Workflow.Data;

        workflowObject.AutoApprove = this.CanAutoApprove(workflowObject);

        return ExecutionResult.Next();
    }

    private bool CanAutoApprove(ApproveOperationWorkflowObject workflowObject)
    {
        return this.contextEvaluator.Evaluate(DBSessionMode.Read, ctx =>
        {
            var permission = ctx.Logics.Permission.GetById(workflowObject.PermissionId, true);

            var operation = ctx.Logics.Operation.GetById(workflowObject.OperationId, true);

            return this.workflowApproveProcessor.CanAutoApprove(permission, operation);
        });
    }
}

public class SetPermissionStep : IStepBody
{
    [NotNull]
    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

    public SetPermissionStep([NotNull] IContextEvaluator<IAuthorizationBLLContext> contextEvaluator)
    {
        this.contextEvaluator = contextEvaluator;
    }

    public PermissionStatus Status { get; set; }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var wfObj = (ApprovePermissionWorkflowObject)context.Workflow.Data;

        this.contextEvaluator.Evaluate(DBSessionMode.Read, ctx =>
        {
            var permission = ctx.Logics.Permission.GetById(wfObj.PermissionId, true);

            permission.Status = this.Status;

            ctx.Logics.Permission.Save(permission);
        });

        wfObj.Status = this.Status;

        return ExecutionResult.Next();
    }
}
