using CommonFramework;

namespace Framework.Validation;

/// <summary>
/// Внутренняя валидация
/// </summary>
/// <typeparam name="TSource">Тип текущего объекта</typeparam>
/// <typeparam name="TProperty">Тип свойства</typeparam>
public class DeepSingleValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
{
    /// <inheritdoc />
    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext) => validationContext.Value.Pipe(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source)));
}