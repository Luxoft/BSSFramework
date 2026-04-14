namespace Framework.Validation.Map;

/// <summary>
/// Коллекция валидаторов
/// </summary>
/// <typeparam name="T">Тип валидатора</typeparam>
public interface IValidatorCollection<out T>
{
    /// <summary>
    /// Валидаторы
    /// </summary>
    IReadOnlyCollection<T> Validators { get; }
}
