using Framework.Validation;

namespace Framework.Workflow.Domain.Definition
{
    public class RequiredStartValidatorAttribute : RequiredValidatorAttribute
    {
        public RequiredStartValidatorAttribute()
        {
            this.OperationContext = WorkflowOperationContextC.Start;
        }
    }
}