using System;

using WorkflowCore.Interface;

namespace Framework.Authorization.ApproveWorkflow;

public class __ApproveOperation_Workflow : IWorkflow<ApproveOperationWorkflowObject>
{
    public void Build(IWorkflowBuilder<ApproveOperationWorkflowObject> builder)
    {
        builder.StartWith(_ => { })
            .Then((_, operation) => Console.WriteLine($"Inner Start {operation.Name}"))
            .Then<CanAutoApproveStep>()
            .If(wfObj => !wfObj.AutoApprove)
            .Do(@if => @if.Parallel()
                .Do(approveBuilder => approveBuilder
                    .WaitFor("Approve_Event", (_, ctx) => ctx.Workflow.Id)
                    .Then((_, wfObj) => wfObj.Status = ApproveOperationWorkflowStatus.Approved)
                    .CancelCondition(operation => operation.Status != ApproveOperationWorkflowStatus.Approving))

                .Do(rejectBuilder => rejectBuilder
                    .WaitFor("Reject_Event", (_, ctx) => ctx.Workflow.Id)
                    .Then((_, operation) => operation.Status = ApproveOperationWorkflowStatus.Rejected)
                    .CancelCondition(operation => operation.Status != ApproveOperationWorkflowStatus.Approving))

                .Join())

            .Then<SendFinalEvent>()
                .Input(step => step.Data, op => op.Status)

            .Then((_, operation) => Console.WriteLine($"Inner Finish {operation.Name}"))
            .EndWorkflow();
    }

    public string Id { get; } = nameof(__ApproveOperation_Workflow);

    public int Version { get; } = 2;
}
