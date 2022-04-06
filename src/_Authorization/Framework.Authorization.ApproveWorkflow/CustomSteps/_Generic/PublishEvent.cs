using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class PublishEvent : IStepBody
{
    private readonly IWorkflowHost host;

    public PublishEvent(IWorkflowHost host)
    {
        this.host = host ?? throw new ArgumentNullException(nameof(host));
    }

    public string EventName { get; set; }

    public string EventKey { get; set; }

    public object EventData { get; set; } = new();


    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        await this.host.PublishEvent(eventName: this.EventName, eventKey: this.EventKey, eventData: this.EventData, DateTime.Now);

        return ExecutionResult.Next();
    }
}
