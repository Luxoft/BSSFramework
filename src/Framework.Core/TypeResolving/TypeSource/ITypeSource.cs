using System.Collections.Immutable;

namespace Framework.Core.TypeResolving.TypeSource;

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
