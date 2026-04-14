using Framework.Validation.Map;

namespace Framework.Validation;

public class SuccessValidator : ValidatorBase
{
    public override ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState ownerState = null) => ValidationResult.Success;
}
