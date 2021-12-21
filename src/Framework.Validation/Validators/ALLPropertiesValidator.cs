using System;

namespace Framework.Validation
{
    public class ALLPropertiesValidator<TProperty> : IPropertyValidator<object, TProperty>
        where TProperty : class
    {
        private readonly int _operationContext;


        public ALLPropertiesValidator(int operationContext)
        {
            this._operationContext = operationContext;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<object, TProperty> context)
        {
            return context.Validator.GetValidationResult(context.Value, this._operationContext);
        }
    }
}