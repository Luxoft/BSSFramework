using System;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class __ApproveOperation_Workflow : IWorkflow<ApproveOperationWorkflowObject>
{
    public void Build(IWorkflowBuilder<ApproveOperationWorkflowObject> builder)
    {
        builder.StartWith(_ => { })
            .Then((_, operation) => Console.WriteLine($"Permission: {operation.PermissionId} | Inner Start {operation.Name}"))
            .Then<CanAutoApproveStep>()
            .If(wfObj => wfObj.AutoApprove)
                .Do(then => then.Then((_, operation) => Console.WriteLine($"Permission: {operation.PermissionId} | AutoApproved"))
                                .Output((_, wfObj) => wfObj.Status = ApproveOperationWorkflowStatus.Approved))
            .If(wfObj => !wfObj.AutoApprove)
                .Do(@if => @if.Parallel()
                    .Do(approveBuilder => this.BuildApproveRejectBranch(approveBuilder, true))
                    .Do(rejectBuilder => this.BuildApproveRejectBranch(rejectBuilder, false))
                    .Join())

            .Then<SendFinalEvent>()
                .Input(step => step.Data, op => op.Status)

            .Then((_, operation) => Console.WriteLine($"Permission: {operation.PermissionId} | Inner Finish {operation.Name}"))
            .EndWorkflow();
    }

    private void BuildApproveRejectBranch(IWorkflowBuilder<ApproveOperationWorkflowObject> branchBuilder, bool isApprove)
    {
        var actionStr = isApprove ? "Approve" : "Reject";

        var loopBranch = branchBuilder.Then((_, operation) => Console.WriteLine($"Permission: {operation.PermissionId} | Start Try{actionStr}"));

        loopBranch
           .Then(isApprove ? (_, wfObj) => wfObj.ApproveEventId = Guid.NewGuid().ToString()
                            : (_, wfObj) => wfObj.RejectEventId = Guid.NewGuid().ToString())
            .WaitFor(GetEventName(isApprove), isApprove ? wfObj => wfObj.ApproveEventId : wfObj => wfObj.RejectEventId, cancelCondition: operation => operation.Status != ApproveOperationWorkflowStatus.Approving)
           .Output(isApprove ? (step, operation) => operation.ApprovedByUser = (string)step.EventData
                             : (step, operation) => operation.RejectedByUser = (string)step.EventData)
            .Then<CalcHasAccessStep>(hasAccessOutput =>
            {
                hasAccessOutput
                    .When(true)
                    .Then(_ => ExecutionResult.Next())
                    .Then((_, operation) =>
                        Console.WriteLine($"Permission: {operation.PermissionId} | hasAccessBuilder"))
                    .Output((_, wfObj) => wfObj.Status = isApprove ? ApproveOperationWorkflowStatus.Approved : ApproveOperationWorkflowStatus.Rejected);


                hasAccessOutput
                    .When(false)
                    .Then(_ => ExecutionResult.Next())
                    .Then((_, operation) => Console.WriteLine($"Permission: {operation.PermissionId} | noAccessBuilder"))
                    .Then<LogAccessErrorStep>()
                        .Input(step => step.IsApprove, _ => isApprove)
                    //.Output(wjObj => wjObj.ApprovedByUser, _ => null)
                    //.Output(wjObj => wjObj.HasApproveAccess, _ => null)

                    .Then(loopBranch);

            })
            .Input(step => step.PrincipalName, isApprove ? wjObj => wjObj.ApprovedByUser : wjObj => wjObj.RejectedByUser);
    }

    public string Id { get; } = nameof(__ApproveOperation_Workflow);

    public int Version { get; } = 2;

    public static string GetEventName(bool isApprove)
    {
        var actionStr = isApprove ? "Approve" : "Reject";

        return $"{actionStr}_Event";
    }
}
