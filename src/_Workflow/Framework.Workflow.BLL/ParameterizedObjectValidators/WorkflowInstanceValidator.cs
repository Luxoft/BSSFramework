using System;

using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class WorkflowInstanceValidator : ParameterizedObjectValidator<Framework.Workflow.Domain.Runtime.WorkflowInstance, Framework.Workflow.Domain.Definition.Workflow, Framework.Workflow.Domain.Runtime.WorkflowInstanceParameter, Framework.Workflow.Domain.Definition.WorkflowParameter>
    {
        public WorkflowInstanceValidator(IWorkflowBLLContext context, ITargetSystemService targetSystemService)
            : base(context, targetSystemService)
        {

        }


        protected override object CreateParameterizedObject(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            return this.TargetSystemService.GetAnonymousObject(workflowInstance);
        }
    }
}