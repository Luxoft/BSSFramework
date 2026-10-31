namespace Framework.Validation.Map;

/// <summary>
/// Validator collection
/// </summary>
/// <typeparam name="T">Validator type</typeparam>
public interface IValidatorCollection<out T>
{
    /// <summary>
    /// Validators
    /// </summary>
    IReadOnlyCollection<T> Validators { get; }
}
