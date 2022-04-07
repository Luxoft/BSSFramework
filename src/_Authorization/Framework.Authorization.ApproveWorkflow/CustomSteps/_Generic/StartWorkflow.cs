using System;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace Framework.Authorization.ApproveWorkflow;

public class StartWorkflow : IStepBody
{
    private readonly IWorkflowHost host;

    public StartWorkflow(IWorkflowHost host)
    {
        this.host = host ?? throw new ArgumentNullException(nameof(host));
    }

    public string WorkflowType { get; set; }

    public object InputData { get; set; }

    public object OutputData { get; set; }

    private string SubWfId { get; set; }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        if (context.ExecutionPointer.EventPublished)
        {
            this.OutputData = context.ExecutionPointer.EventData;
            this.SubWfId = context.ExecutionPointer.EventKey;

            return ExecutionResult.Next();
        }

        this.SubWfId = await this.host.StartWorkflow(workflowId: this.WorkflowType, data: this.InputData);

        return new WaitFor
        {
            EventName = SendFinalEvent.EventName,
            EventKey = this.SubWfId
        }.Run(context);
    }
}
