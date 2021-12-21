using Framework.Core;

using Framework.Validation;


namespace Framework.Workflow.Domain.Definition
{
    public class WorkflowElementPropertyValidator : IPropertyValidator<IWorkflowElement, IWorkflowElement>
    {
        private WorkflowElementPropertyValidator()
        {

        }

        public ValidationResult GetValidationResult(IPropertyValidationContext<IWorkflowElement, IWorkflowElement> context)
        {
            var propertyHasInvalidWorkflowRequest = from value in context.Value.ToMaybe()

                                                    from valueWf in value.Workflow.ToMaybe()

                                                    let source = context.Source

                                                    where source.Workflow != valueWf

                                                    select new
                                                    {
                                                        SourceWorkflow = source.Workflow,
                                                        PropertyWorkflow = valueWf
                                                    };

            var invalidWorkfolwData = propertyHasInvalidWorkflowRequest.GetValueOrDefault();

            return ValidationResult.FromCondition(invalidWorkfolwData == null,

                () =>
                $"Property {context.GetPropertyName()} of object {context.GetSourceTypeName()} refers to \"{invalidWorkfolwData.PropertyWorkflow.Name}\", but must belong to \"{invalidWorkfolwData.SourceWorkflow.Name}\"");
        }


        public static WorkflowElementPropertyValidator Value { get; } = new WorkflowElementPropertyValidator();
    }
}