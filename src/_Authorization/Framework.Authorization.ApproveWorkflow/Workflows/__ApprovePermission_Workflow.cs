using System;
using System.Linq;

using Framework.Authorization.Domain;

using WorkflowCore.Interface;
using WorkflowCore.Primitives;

namespace Framework.Authorization.ApproveWorkflow;

public class __ApprovePermission_Workflow : IWorkflow<ApprovePermissionWorkflowObject>
{
    public string Id { get; } = nameof(__ApprovePermission_Workflow);

    public int Version { get; } = 1;

    public void Build(IWorkflowBuilder<ApprovePermissionWorkflowObject> builder)
    {
        builder
            .StartWith(_ => { })
            .Then<SetPermissionStatusStep>()
                .Input(step => step.Status, _ => PermissionStatus.Approving)

            .Parallel()
                .Do(x => x
                    .ForEach(permission => permission.Operations, runParallel: _ => true)
                        .Do(this.ProcessSubWorkflow)
                        .CancelCondition(permission => permission.SomeOneOperationRejected))
            .Join()

            .Then<SetPermissionStatusStep>()
                .Input(step => step.Status, permission => permission.Operations.All(operation => operation.Status == ApproveOperationWorkflowStatus.Approved)
                                                                  ? PermissionStatus.Approved
                                                                  : PermissionStatus.Rejected)

            .ForEach(permission => permission.Operations.Where(op => op.Status == ApproveOperationWorkflowStatus.Approving))
            .Do(iterateBuilder => iterateBuilder
                                  .Then<TerminateWorkflowStep>()
                                  .Input(step => step.WorkflowInstanceId, (_, ctx) => ctx.GetOperation().WorkflowInstanceId))

            .Then(_ => Console.WriteLine("end permission"))
            .EndWorkflow();
    }

    private void ProcessSubWorkflow(IWorkflowBuilder<ApprovePermissionWorkflowObject> builder)
    {
        builder

                .Then((context, p) => Console.WriteLine($"Permission: {p.PermissionId} | [Start {context.GetOperation().Name}]"))
                .Then<StartWorkflow>()
                .Input(step => step.WorkflowType, ctx => nameof(__ApproveOperation_Workflow))
                .Input(step => step.InputData, (_, ctx) => ctx.Item)
                .Output((step, permission) => permission.GetActualItem(step.InputData).WorkflowInstanceId = step.SubWfId)

                .Then<WaitFor>()
                .Input(step => step.EventName, _ => SendFinalEvent.EventName)
                .Input(step => step.EventKey, (permission, ctx) => ctx.GetOperation().WorkflowInstanceId)

                .Output((step, permission) =>
                        {
                            var operation = permission.Operations.Single(op => op.WorkflowInstanceId == step.EventKey);

                            operation.Status = (ApproveOperationWorkflowStatus)Convert.ToInt32(step.EventData);
                        })

                .Then((context, p) => Console.WriteLine($"Permission: {p.PermissionId} | [Finish {context.GetOperation().Name}] | Operation Status: {context.GetOperation().Status}"))
                .Then((ctx, permission) =>
                      {
                          if (ctx.GetOperation().Status == ApproveOperationWorkflowStatus.Rejected)
                          {
                              permission.SomeOneOperationRejected = true;
                          }
                      });
    }
}
