using System.Collections.Immutable;

// ReSharper disable once CheckNamespace
namespace Framework.Core.TypeResolving;

/// <summary>
/// Type source
/// </summary>
public interface ITypeSource
{
    /// <summary>
    /// Getting types
    /// </summary>
    /// <returns></returns>
    ImmutableHashSet<Type> Types { get; }
}
