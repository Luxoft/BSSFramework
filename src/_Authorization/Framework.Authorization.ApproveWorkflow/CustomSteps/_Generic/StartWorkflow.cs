using WorkflowCore.Interface;
using WorkflowCore.Models;

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

    public string SubWfId { get; set; }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        if (!context.ExecutionPointer.EventPublished)
        {
            this.SubWfId = await this.host.StartWorkflow(workflowId: this.WorkflowType, data: this.InputData);
        }

        return ExecutionResult.Next();
    }
}
