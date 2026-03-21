namespace Framework.Validation;

/// <summary>
/// Текущая позиция валидации
/// </summary>
public record ValidationState(IValidationState? Parent, IPropertyValidationMap PropertyMap, object Source) : IValidationState
{
    /// <summary>
    /// Валидируемый тип
    /// </summary>
    public IClassValidationMap SourceTypeMap => this.PropertyMap.ReflectedTypeMap;
}
