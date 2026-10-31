namespace Framework.Projection.Lambda;

/// <summary>
/// Object with additional attributes for generation
/// </summary>
public interface IProjectionAttributeProvider
{
    /// <summary>
    /// Additional generation attributes
    /// </summary>
    IReadOnlyList<Attribute> Attributes { get; }
}
