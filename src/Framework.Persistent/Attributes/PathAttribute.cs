using System;

using Framework.Core;

namespace Framework.Persistent;

/// <summary>
/// Атрибут пути объкта
/// </summary>
public abstract class PathAttribute : Attribute, IPathAttribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Путь</param>
    protected PathAttribute(PropertyPath path)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        this.Path = path.ToString();
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Путь</param>
    protected PathAttribute(string path)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        this.Path = path;
    }

    /// <summary>
    /// Путь
    /// </summary>
    public string Path { get; }
}
