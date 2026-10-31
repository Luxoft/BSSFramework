using Framework.Validation.Map;

namespace Framework.Validation.Validators.Deep;

/// <summary>
/// Internal collection validation
/// </summary>
/// <typeparam name="TSource">Current object type</typeparam>
/// <typeparam name="TProperty">Property type</typeparam>
/// <typeparam name="TElement">Property element type</typeparam>
public class DeepCollectionValidator<TSource, TProperty, TElement> : IPropertyValidator<TSource, TProperty>
        where TProperty : IEnumerable<TElement>
{
    /// <inheritdoc />
    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext) => validationContext.Value.Sum(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source!)));
}
