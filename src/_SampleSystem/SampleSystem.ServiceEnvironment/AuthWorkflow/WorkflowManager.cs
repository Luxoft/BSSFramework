using WorkflowCore.Interface;

namespace SampleSystem.ServiceEnvironment;

public class WorkflowManager : IWorkflowManager
{
    private readonly IWorkflowHost workflowHost;

    public WorkflowManager(IWorkflowHost workflowHost)
    {
        this.workflowHost = workflowHost;
    }


    public bool Enabled { get; private set; }

    public void Start()
    {
        this.workflowHost.Start();

        this.Enabled = true;
    }
    public void Stop()
    {
        this.workflowHost.Stop();

        this.Enabled = false;
    }
}
