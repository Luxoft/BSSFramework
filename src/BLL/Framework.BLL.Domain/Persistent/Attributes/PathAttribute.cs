namespace Framework.BLL.Domain.Persistent.Attributes;

/// <summary>
/// Object path attribute
/// </summary>
public abstract class PathAttribute(string path) : Attribute, IPathAttribute
{
    /// <summary>
    /// Path
    /// </summary>
    public string Path { get; } = path;
}
