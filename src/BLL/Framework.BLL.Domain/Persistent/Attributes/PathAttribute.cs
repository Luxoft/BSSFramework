namespace Framework.BLL.Domain.Persistent.Attributes;

/// <summary>
/// Атрибут пути объкта
/// </summary>
public abstract class PathAttribute(string path) : Attribute, IPathAttribute
{
    /// <summary>
    /// Путь
    /// </summary>
    public string Path { get; } = path;
}
