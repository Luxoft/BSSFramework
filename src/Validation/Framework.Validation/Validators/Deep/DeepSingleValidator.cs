using Anch.Core;

using Framework.Validation.Map;

namespace Framework.Validation.Validators.Deep;

/// <summary>
/// Internal validation
/// </summary>
/// <typeparam name="TSource">Current object type</typeparam>
/// <typeparam name="TProperty">Property type</typeparam>
public class DeepSingleValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
{
    /// <inheritdoc />
    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext) => validationContext.Value.Pipe(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source!)));
}
