using CommonFramework;

namespace Framework.Validation;

public static class ValidatorExtensions
{
    public static void Validate<TSource>(this IValidator validator, TSource source, int operationContext = int.MaxValue, IValidationState parentState = null)
            where TSource : class
    {
        if (validator == null) throw new ArgumentNullException(nameof(validator));
        if (source == null) throw new ArgumentNullException(nameof(source));

        validator.GetValidationResult(source, operationContext, parentState).TryThrow();
    }

    public static ValidationResult GetDynamicValidateResult(this IValidator validator, object source, int operationContext = int.MaxValue, IValidationState parentState = null)
    {
        if (validator == null) throw new ArgumentNullException(nameof(validator));

        if (source != null)
        {
            var sourceType = source.GetType();

            if (sourceType != typeof(object))
            {
                return new Func<object, int, IValidationState, ValidationResult>(validator.GetValidationResult)
                       .CreateGenericMethod(sourceType)
                       .Invoke<ValidationResult>(validator, source, operationContext, parentState);
            }
        }

        return ValidationResult.Success;
    }
}
