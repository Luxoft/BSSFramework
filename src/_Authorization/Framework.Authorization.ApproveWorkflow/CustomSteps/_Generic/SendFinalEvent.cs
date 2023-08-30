using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class SendFinalEvent : IStepBody
{
    private readonly PublishEvent publishEvent;

    public SendFinalEvent(PublishEvent publishEvent)
    {
        this.publishEvent = publishEvent;
    }

    public object Data { get; set; }

    public Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        this.publishEvent.EventName = EventName;
        this.publishEvent.EventKey = context.Workflow.Id;
        this.publishEvent.EventData = this.Data;

        return this.publishEvent.RunAsync(context);
    }

    public static readonly string EventName = "Finish Workflow";
}
