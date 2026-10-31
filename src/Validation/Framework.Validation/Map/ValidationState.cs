namespace Framework.Validation.Map;

/// <summary>
/// Current validation position
/// </summary>
public record ValidationState(IValidationState? Parent, IPropertyValidationMap PropertyMap, object Source) : IValidationState
{
    /// <summary>
    /// Validated type
    /// </summary>
    public IClassValidationMap SourceTypeMap => this.PropertyMap.ReflectedTypeMap;
}
