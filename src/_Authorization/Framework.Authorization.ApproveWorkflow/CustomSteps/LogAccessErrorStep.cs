using System;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class LogAccessErrorStep : IStepBody
{
    private readonly IPersistenceProvider persistenceProvider;

    public LogAccessErrorStep(IPersistenceProvider persistenceProvider)
    {
        this.persistenceProvider = persistenceProvider;
    }

    public bool IsApprove { get; set; }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var wfObj = (ApproveOperationWorkflowObject)context.Workflow.Data;

        await this.persistenceProvider.PersistErrors(new[]
        {
            new ExecutionError
            {
                WorkflowId = context.Workflow.Id,
                ErrorTime = DateTime.Now,
                ExecutionPointerId = context.ExecutionPointer.Id,
                Message = this.IsApprove
                    ? $"Permission:{wfObj.PermissionId} | Access denied with eventId {wfObj.ApproveEventId}"
                    : $"Permission:{wfObj.PermissionId} | Access denied with eventId {wfObj.RejectEventId}"
            }
        });

        return ExecutionResult.Next();
    }
}
