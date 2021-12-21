using System;

using Framework.Validation;


namespace Framework.Workflow.Domain.Definition
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WorkflowTargetSystemValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return WorkflowTargetSystemPropertyValidator.Value;
        }
    }
}