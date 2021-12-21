using Framework.Core;
using Framework.Persistent;
using Framework.Validation;


namespace Framework.Workflow.Domain.Definition
{
    public class WorkflowTargetSystemPropertyValidator : IPropertyValidator<IWorkflowElement, ITargetSystemElement<TargetSystem>>
    {
        private WorkflowTargetSystemPropertyValidator()
        {

        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<IWorkflowElement, ITargetSystemElement<TargetSystem>> context)
        {
            var invalidDataRequest = from value in context.Value.ToMaybe()

                                     from valueTargetSystem in value.TargetSystem.ToMaybe()

                                     where !valueTargetSystem.IsBase

                                     let workflowTargetSystem = context.Source.Workflow.TargetSystem

                                     where workflowTargetSystem != valueTargetSystem

                                     select
                                         $"Property {context.GetPropertyName()} of object {context.Source.ToFormattedString(context.GetSourceTypeName())} refers to \"{valueTargetSystem.Name}\", but must belong to \"{workflowTargetSystem.Name}\"";


            return ValidationResult.FromMaybe(invalidDataRequest);
        }


        public static WorkflowTargetSystemPropertyValidator Value { get; } = new WorkflowTargetSystemPropertyValidator();
    }
}