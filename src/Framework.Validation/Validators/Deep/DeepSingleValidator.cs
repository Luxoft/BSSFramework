using Framework.Core;

namespace Framework.Validation
{
    /// <summary>
    /// ¬нутренн€€ валидаци€
    /// </summary>
    /// <typeparam name="TSource">“ип текущего объекта</typeparam>
    /// <typeparam name="TProperty">“ип свойства</typeparam>
    public class DeepSingleValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
    {
        /// <inheritdoc />
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
        {
            return validationContext.Value.Pipe(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source)));
        }
    }
}