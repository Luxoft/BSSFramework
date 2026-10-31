using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.BLL.Domain.Attributes;

/// <summary>
/// Attribute indicating that security is used from another type
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependencySecurityAttribute : Attribute, IPathAttribute
{
    /// <summary>
    /// Attribute constructor
    /// </summary>
    /// <param name="sourceType">Type of the source object in which security is defined</param>
    /// <param name="path">Path to the source object in which security is defined; if left empty, comparison will be performed untyped by Ids (relevant for projections and views)</param>
    public DependencySecurityAttribute(Type sourceType, string path = "")
    {
        this.SourceType = sourceType;
        this.Path = path;
    }


    /// <summary>
    /// Type of the source object in which security is defined
    /// </summary>
    public Type SourceType { get; }

    /// <summary>
    /// Path to the source object in which security is defined
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Whether the inherited security is untyped (for projections and views)
    /// </summary>
    public bool IsUntyped => this.Path == string.Empty;
}
