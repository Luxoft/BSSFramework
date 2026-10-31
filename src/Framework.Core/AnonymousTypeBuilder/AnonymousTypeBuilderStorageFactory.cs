using System.Collections.Concurrent;

namespace Framework.Core.AnonymousTypeBuilder;

/// <summary>
/// Factory for creating <see cref="AnonymousTypeBuilderStorage"/>. Caches the created values
/// </summary>
public class AnonymousTypeBuilderStorageFactory
{
    private static readonly ConcurrentDictionary<string, IAnonymousTypeBuilderStorage> ConcurrentDictionary = new();

    public IAnonymousTypeBuilderStorage Create(string assemblyBuilderName) =>
        ConcurrentDictionary.GetOrAdd(
            assemblyBuilderName,
            builderName => new AnonymousTypeBuilderStorage(builderName));
}
