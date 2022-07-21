//using System;
//using System.Threading.Tasks;

//using Framework.Authorization.BLL;
//using Framework.DomainDriven.BLL;

//using JetBrains.Annotations;

//using Microsoft.Extensions.DependencyInjection;

//using WorkflowCore.Interface;
//using WorkflowCore.Models;

//namespace Framework.Authorization.ApproveWorkflow;

//public class CanAutoApproveStep : IStepBody
//{
//    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

//    public CanAutoApproveStep([NotNull] IContextEvaluator<IAuthorizationBLLContext> contextEvaluator)
//    {
//        this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
//    }

//    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
//    {
//        var workflowObject = (ApproveOperationWorkflowObject)context.Workflow.Data;

//        workflowObject.AutoApprove = this.CanAutoApprove(workflowObject);

//        return ExecutionResult.Next();
//    }

//    private bool CanAutoApprove(ApproveOperationWorkflowObject workflowObject)
//    {
//        return this.contextEvaluator.Evaluate(DBSessionMode.Read, ctx =>
//        {
//            var permission = ctx.Logics.Permission.GetById(workflowObject.PermissionId, true);

//            var operation = ctx.Logics.Operation.GetById(workflowObject.OperationId, true);

//            return ctx.ServiceProvider.GetRequiredService<IWorkflowApproveProcessor>().CanAutoApprove(permission, operation);
//        });
//    }
//}
