// ReSharper disable once CheckNamespace
namespace Framework.Database;

/// <summary>
/// For proxy types
/// </summary>
public interface IDALObject
{
    object Object { get; }

    Type Type { get; }

    /// <summary>
    /// Numeric identifier that determines the order of applying/changing/retrieving this entity
    /// </summary>
    long ApplyIndex { get; }
}
