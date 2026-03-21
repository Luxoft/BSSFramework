namespace Framework.Validation;

public interface IElementValidator<in TValidationContext>
{
    /// <summary>
    /// Получение результата валидации
    /// </summary>
    /// <param name="validationContext">Валидационный контекст</param>
    /// <returns></returns>
    ValidationResult GetValidationResult(TValidationContext validationContext);
}
