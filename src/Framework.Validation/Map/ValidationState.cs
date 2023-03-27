using JetBrains.Annotations;

namespace Framework.Validation;

/// <summary>
/// Текущая позиция валидации
/// </summary>
public class ValidationState : IValidationState
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="parent">Стейт верхнего уровня</param>
    /// <param name="propertyMap">Валидируемое свойство</param>
    /// <param name="source">Валидируемый объект</param>
    public ValidationState(IValidationState parent, [NotNull] IPropertyValidationMap propertyMap, [NotNull] object source)
    {
        if (propertyMap == null) throw new ArgumentNullException(nameof(propertyMap));
        if (source == null) throw new ArgumentNullException(nameof(source));

        this.Parent = parent;
        this.PropertyMap = propertyMap;
        this.Source = source;
    }

    /// <summary>
    /// Стейт верхнего уровня
    /// </summary>
    public IValidationState Parent { get; }

    /// <summary>
    /// Валидируемое свойство
    /// </summary>
    public IPropertyValidationMap PropertyMap { get; }

    /// <summary>
    /// Валидируемый объект
    /// </summary>
    public object Source { get; }

    /// <summary>
    /// Валидируемый тип
    /// </summary>
    public IClassValidationMap SourceTypeMap => this.PropertyMap.ReflectedTypeMap;
}
