using System;
using System.Linq;

using Framework.Authorization.Domain;

using WorkflowCore.Interface;

namespace Framework.Authorization.ApproveWorkflow;

public class __ApprovePermission_Workflow : IWorkflow<ApprovePermissionWorkflowObject>
{
    public string Id { get; } = nameof(__ApprovePermission_Workflow);

    public int Version { get; } = 1;

    public void Build(IWorkflowBuilder<ApprovePermissionWorkflowObject> builder)
    {
        builder
            .StartWith(_ => { })
            .Then<SetPermissionStep>()
                .Input(step => step.Status, _ => PermissionStatus.Approving)

            .Parallel()
                .Do(x => x
                    .ForEach(permission => permission.Operations, runParallel: _ => true)
                        .Do(this.ProcessSubWorkflow)
                        .CancelCondition(permission => permission.SomeOneOperationRejected))
            .Join()

            .Then((_, permission) => permission.Status =
                permission.Operations.All(operation => operation.Status == ApproveOperationWorkflowStatus.Approved)
                    ? PermissionStatus.Approved
                    : PermissionStatus.Rejected)
            .Then(_ => Console.WriteLine("end permission"))
            .EndWorkflow();
    }

    private void ProcessSubWorkflow(IWorkflowBuilder<ApprovePermissionWorkflowObject> builder)
    {
        builder.StartWith(_ => { })

            .Then(context => Console.WriteLine($"[Start {context.GetOperation().Name}]"))
            .Then<StartWorkflow>()
            .Input(step => step.WorkflowType, ctx => nameof(__ApproveOperation_Workflow))
            .Input(step => step.InputData, (_, ctx) => ctx.Item)
            .Output((step, permission) =>
            {
                var operationId = ((ApproveOperationWorkflowObject)step.InputData).OperationId;

                permission.Operations.Single(op => op.OperationId == operationId).Status =
                    (ApproveOperationWorkflowStatus)Convert.ToInt32(step.OutputData);
            })

            .Then(context =>
                Console.WriteLine(
                    $"[Finish {context.GetOperation().Name}] | Operation Status: {context.GetOperation().Status}"))
            .Then((ctx, permission) =>
            {
                if (ctx.GetOperation().Status == ApproveOperationWorkflowStatus.Rejected)
                {
                    permission.SomeOneOperationRejected = true;
                }
            });
    }
}
