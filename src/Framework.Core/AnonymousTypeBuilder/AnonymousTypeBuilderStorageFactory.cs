using System.Collections.Concurrent;

namespace Framework.Core.AnonymousTypeBuilder;

/// <summary>
/// Фабрика создания <see cref="AnonymousTypeBuilderStorage"/>. Кэширует создаваемые значения
/// </summary>
public class AnonymousTypeBuilderStorageFactory
{
    private static readonly ConcurrentDictionary<string, IAnonymousTypeBuilderStorage> ConcurrentDictionary = new();

    public IAnonymousTypeBuilderStorage Create(string assemblyBuilderName) =>
        ConcurrentDictionary.GetOrAdd(
            assemblyBuilderName,
            builderName => new AnonymousTypeBuilderStorage(builderName));
}
