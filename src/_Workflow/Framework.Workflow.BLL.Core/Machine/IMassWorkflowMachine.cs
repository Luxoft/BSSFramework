using Framework.Core;

namespace Framework.Workflow.BLL
{
    public interface IMassWorkflowMachine
    {
        ITryResult<WorkflowProcessResult>[] ProcessTimeouts();
    }
}