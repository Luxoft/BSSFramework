namespace Framework.Validation;

public abstract class ValidatorBase : IValidator
{
    public abstract ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState? ownerState = null);


    public static readonly IValidator Success = new SuccessValidator();
}

public class Validator(ValidatorCompileCache cache) : ValidatorBase
{
    public override ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState? ownerState = null)
    {
        return cache.GetValidationResult(new ValidationContextBase<TSource>(this, operationContext, source, ownerState));
    }
}
