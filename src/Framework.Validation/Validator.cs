namespace Framework.Validation;

public class Validator(ValidatorCompileCache cache) : ValidatorBase
{
    public override ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue)
    {
        return cache.GetValidationResult(new ValidationContextBase<TSource>(this, operationContext, source, ownerState));
    }
}
