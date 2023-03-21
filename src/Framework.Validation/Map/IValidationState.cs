namespace Framework.Validation;

/// <summary>
/// Текущая позиция валидации
/// </summary>
public interface IValidationState
{
    /// <summary>
    /// Стейт верхнего уровня
    /// </summary>
    IValidationState Parent { get; }

    /// <summary>
    /// Валидируемое свойство
    /// </summary>
    IPropertyValidationMap PropertyMap { get; }

    /// <summary>
    /// Валидируемый объект
    /// </summary>
    object Source { get; }

    /// <summary>
    /// Валидируемый тип
    /// </summary>
    IClassValidationMap SourceTypeMap { get; }
}
