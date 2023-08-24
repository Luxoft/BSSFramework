namespace Framework.DomainDriven.Tracking;

/// <summary>
/// Defines GetChanges method working mode for not persistent objects
/// </summary>
public enum GetChangesMode
{
    /// <summary>
    /// Gets all changes without validation values to be dafault ones. This is default behavior by compatibility reasons
    /// </summary>
    Default,

    /// <summary>
    /// Gets only really changed properties on new object, i.e. default property values ignored
    /// </summary>
    IgnoreDefaultValues
}
