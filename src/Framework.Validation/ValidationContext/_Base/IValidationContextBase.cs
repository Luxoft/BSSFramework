namespace Framework.Validation;

/// <summary>
/// Контекст валидации
/// </summary>
public interface IValidationContextBase : IValidatorContainer<IValidator>, IOperationContextData
{
    /// <summary>
    /// Верхний (по стеку) стейт валидации
    /// </summary>
    IValidationState ParentState { get; }
}

/// <summary>
/// Контекст валидации
/// </summary>
/// <typeparam name="TSource">Тип объекта</typeparam>
public interface IValidationContextBase<out TSource> : IValidationContextBase
{
    /// <summary>
    /// Валидируемый объект
    /// </summary>
    TSource Source { get; }
}

/// <summary>
/// Контекст валидации
/// </summary>
/// <typeparam name="TSource">Тип объекта</typeparam>
/// <typeparam name="TValidationMap">Метаданные</typeparam>
public interface IValidationContextBase<out TSource, out TValidationMap> : IValidationContextBase<TSource>
{
    TValidationMap Map { get; }
}
