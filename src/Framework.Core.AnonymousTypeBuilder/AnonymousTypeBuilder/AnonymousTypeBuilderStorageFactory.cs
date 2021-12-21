using System.Collections.Concurrent;

namespace Framework.Core
{
    /// <summary>
    /// Фабрика создания <see cref="AnonymousTypeBuilderStorage"/>. Кэширует создаваемые значения
    /// </summary>
    public class AnonymousTypeBuilderStorageFactory
    {
        private static readonly ConcurrentDictionary<string, IAnonymousTypeBuilderStorage> ConcurrentDictionary = new ConcurrentDictionary<string, IAnonymousTypeBuilderStorage>();

        public IAnonymousTypeBuilderStorage Create(string assemblyBuilderName)
        {
            return AnonymousTypeBuilderStorageFactory.ConcurrentDictionary.GetOrAdd(
                                                                                    assemblyBuilderName,
                                                                                    builderName => new AnonymousTypeBuilderStorage(builderName));
        }
    }
}
