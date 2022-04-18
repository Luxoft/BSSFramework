using System;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class TerminateWorkflowStep : IStepBody
{
    private readonly IWorkflowHost host;

    public TerminateWorkflowStep(IWorkflowHost host)
    {
        this.host = host ?? throw new ArgumentNullException(nameof(host));
    }

    public string WorkflowInstanceId { get; set; }

    public bool Terminated { get; set; }


    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        this.Terminated = await this.host.TerminateWorkflow(this.WorkflowInstanceId);

        return ExecutionResult.Next();
    }
}
