namespace Framework.Validation.Validators;

public interface IElementValidator<in TValidationContext>
{
    /// <summary>
    /// Getting the validation result
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns></returns>
    ValidationResult GetValidationResult(TValidationContext validationContext);
}
