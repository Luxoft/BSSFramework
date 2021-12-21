using Framework.Workflow.Domain;

namespace Framework.Workflow.BLL
{
    public abstract class WorfklowMachineEventProcessor<TDomainObject> : WorfklowEventProcessor<TDomainObject>
        where TDomainObject : DomainObjectBase
    {
        protected readonly IWorkflowMachine WorkflowMachine;


        protected WorfklowMachineEventProcessor(IWorkflowBLLContext context, IWorkflowMachine workflowMachine)
            : base(context)
        {
            this.WorkflowMachine = workflowMachine;
        }
    }
}