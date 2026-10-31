namespace Framework.Projection.Lambda.ProjectionSource.AttributeSource;

/// <summary>
/// Attribute source for projection types/properties
/// </summary>
public interface IAttributeSource
{
    /// <summary>
    /// Getting all object attributes
    /// </summary>
    /// <returns></returns>
    IEnumerable<Attribute> GetAttributes();
}
