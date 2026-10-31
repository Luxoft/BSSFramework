namespace Framework.BLL.Domain.DirectMode;

/// <summary>
/// Attribute controlling model-based generation
/// </summary>
public class DirectModeAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="directMode">Model-based generation control parameters</param>
    public DirectModeAttribute(DirectMode directMode) => this.DirectMode = directMode;

    /// <summary>
    /// Model-based generation control parameters
    /// </summary>
    public DirectMode DirectMode { get; }
}
