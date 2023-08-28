using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.DomainDriven;



using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class SetPermissionStatusStep : IStepBody
{
    
    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

    public SetPermissionStatusStep(IContextEvaluator<IAuthorizationBLLContext> contextEvaluator)
    {
        this.contextEvaluator = contextEvaluator;
    }

    public PermissionStatus Status { get; set; }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var wfObj = (ApprovePermissionWorkflowObject)context.Workflow.Data;

        this.contextEvaluator.Evaluate (DBSessionMode.Write, ctx =>
        {
            var permission = ctx.Logics.Permission.GetById(wfObj.PermissionId, true);

            permission.Status = this.Status;

            ctx.Logics.Permission.Save(permission);
        });

        wfObj.Status = this.Status;

        return ExecutionResult.Next();
    }
}
