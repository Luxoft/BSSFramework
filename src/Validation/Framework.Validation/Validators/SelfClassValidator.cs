namespace Framework.Validation.Validators;

public class SelfClassValidator<TSource> : IClassValidator<TSource>
        where TSource : IClassValidator<TSource>
{
    public ValidationResult GetValidationResult(IClassValidationContext<TSource> validationContext)
    {
        if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

        return validationContext.Source.GetValidationResult(validationContext);
    }
}
