using Framework.Authorization.BLL;
using Framework.DomainDriven;

using JetBrains.Annotations;

using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace Framework.Authorization.ApproveWorkflow;

public class CalcHasAccessStep : WaitFor
{
    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

    public CalcHasAccessStep([NotNull] IContextEvaluator<IAuthorizationBLLContext> contextEvaluator)
    {
        this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
    }

    public string PrincipalName { get; set; }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        var wfObj = (ApproveOperationWorkflowObject)context.Workflow.Data;

        var hasAccess = this.HasAccess(wfObj);

        return ExecutionResult.Outcome(hasAccess);
    }

    private bool HasAccess(ApproveOperationWorkflowObject workflowObject)
    {
        return this.contextEvaluator.Evaluate(DBSessionMode.Read, this.PrincipalName, ctx =>
        {
            var operation = ctx.Logics.Operation.GetById(workflowObject.OperationId, true);

            return ctx.GetOperationSecurityProvider().HasAccess(operation);
        });
    }
}
