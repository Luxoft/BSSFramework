using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial interface IWorkflowBLLContext
    {
        IWorkflowMachine StartWorkflowMachine(StartWorkflowRequest request);

        IWorkflowMachine GetWorkflowMachine(WorkflowInstance workflowInstance);

        IMassWorkflowMachine GetMassWorkflowMachine(Workflow.Domain.Definition.Workflow definition, WorkflowInstance[] workflowInstances);

        WorkflowProcessResult FinishParallels(WorkflowProcessResult processResult);
    }
}