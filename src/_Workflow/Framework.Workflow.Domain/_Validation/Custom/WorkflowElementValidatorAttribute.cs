using System;

using Framework.Validation;


namespace Framework.Workflow.Domain.Definition
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WorkflowElementValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return WorkflowElementPropertyValidator.Value;
        }
    }
}