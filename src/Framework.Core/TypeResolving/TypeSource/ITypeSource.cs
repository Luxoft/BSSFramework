using System.Collections.Immutable;

// ReSharper disable once CheckNamespace
namespace Framework.Core.TypeResolving;

/// <summary>
/// Источник типов
/// </summary>
public interface ITypeSource
{
    /// <summary>
    /// Получение типов
    /// </summary>
    /// <returns></returns>
    ImmutableHashSet<Type> Types { get; }
}
