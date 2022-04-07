using System;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class CanAutoApproveStep : IStepBody
{
    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

    private readonly IWorkflowApproveProcessor workflowApproveProcessor;

    public CanAutoApproveStep([NotNull] IScopedContextEvaluator<IAuthorizationBLLContext> contextEvaluator,
                              [NotNull] IWorkflowApproveProcessor workflowApproveProcessor)
    {
        this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
        this.workflowApproveProcessor = workflowApproveProcessor ?? throw new ArgumentNullException(nameof(workflowApproveProcessor));
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
