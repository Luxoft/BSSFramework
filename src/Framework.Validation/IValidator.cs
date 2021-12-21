namespace Framework.Validation
{
    public interface IValidator
    {
        ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState ownerState = null);
    }
}