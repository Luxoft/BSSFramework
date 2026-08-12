using Framework.Validation.Map;

namespace Framework.Validation;

public class Validator(ValidatorCompileCache cache) : ValidatorBase
{
    public override ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState? ownerState = null) => cache.GetValidationResult(new ValidationContextBase<TSource>(this, operationContext, source, ownerState));
}
