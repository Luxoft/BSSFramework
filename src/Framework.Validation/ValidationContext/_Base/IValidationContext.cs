namespace Framework.Validation
{
    /// <summary>
    /// Контекст валидации
    /// </summary>
    public interface IValidationContext : IValidationContextBase, IExtendedValidationDataContainer
    {
    }

    /// <summary>
    /// Контекст валидации
    /// </summary>
    /// <typeparam name="TSource">Тип объекта</typeparam>
    public interface IValidationContext<out TSource> : IValidationContextBase<TSource>, IValidationContext
    {
    }

    /// <summary>
    /// Контекст валидации
    /// </summary>
    /// <typeparam name="TSource">Тип объекта</typeparam>
    /// <typeparam name="TValidationMap">Метаданные</typeparam>
    public interface IValidationContext<out TSource, out TValidationMap> : IValidationContext<TSource>, IValidationContextBase<TSource, TValidationMap>
        where TValidationMap : class
    {
    }
}