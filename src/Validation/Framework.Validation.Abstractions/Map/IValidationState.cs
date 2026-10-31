namespace Framework.Validation.Map;

/// <summary>
/// Current validation position
/// </summary>
public interface IValidationState
{
    /// <summary>
    /// Top-level state
    /// </summary>
    IValidationState? Parent { get; }

    /// <summary>
    /// Validated property
    /// </summary>
    IPropertyValidationMap PropertyMap { get; }

    /// <summary>
    /// Validated object
    /// </summary>
    object Source { get; }

    /// <summary>
    /// Validated type
    /// </summary>
    IClassValidationMap SourceTypeMap { get; }
}
