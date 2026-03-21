namespace Framework.Validation;

public abstract class ValidatorBase : IValidator
{
    public abstract ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue);


    public static readonly IValidator Success = new SuccessValidator();
}
