using Framework.Core;


namespace Framework.BLL.Domain.Persistent.Attributes;

/// <summary>
/// Атрибут пути объкта
/// </summary>
public abstract class PathAttribute(string path) : Attribute, IPathAttribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Путь</param>
    protected PathAttribute(PropertyPath path)
        : this(path.ToString())
    {
    }

    /// <summary>
    /// Путь
    /// </summary>
    public string Path { get; } = path;
}
