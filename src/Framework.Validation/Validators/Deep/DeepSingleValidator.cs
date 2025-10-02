using CommonFramework;

namespace Framework.Validation
{
    /// <summary>
    /// Âíóòðåííÿÿ âàëèäàöèÿ
    /// </summary>
    /// <typeparam name="TSource">Òèï òåêóùåãî îáúåêòà</typeparam>
    /// <typeparam name="TProperty">Òèï ñâîéñòâà</typeparam>
    public class DeepSingleValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
    {
        /// <inheritdoc />
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
        {
            return validationContext.Value.Pipe(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source)));
        }
    }
}
