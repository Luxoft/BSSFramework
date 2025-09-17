using CommonFramework;

using Framework.Configuration.SubscriptionModeling;
using Framework.Core;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
///     Хранилище описаний подписок.
/// </summary>
public class SubscriptionMetadataStore
{
    internal readonly ILookup<Type, ISubscriptionMetadata> store;

    /// <summary>
    ///     Создаёт экземпляр класса <see cref="SubscriptionMetadataStore" />.
    /// </summary>
    /// <param name="subscriptionMetadataFinder">Экземпляр компонента для поиска моделей подписок.</param>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент <paramref name="subscriptionMetadataFinder" /> равен null.
    /// </exception>
    public SubscriptionMetadataStore(ISubscriptionMetadataFinder subscriptionMetadataFinder)
    {
        if (subscriptionMetadataFinder == null)
        {
            throw new ArgumentNullException(nameof(subscriptionMetadataFinder));
        }

        this.store = subscriptionMetadataFinder
                     .Find()
                     .EmptyIfNull()
                     .Select(m => { m.Validate(); return m; })
                     .ToLookup(m => m.DomainObjectType);
    }

    /// <summary>
    ///     Возвращает все сохранённые в хранилище описания подписок по указанному типу доменного объекта.
    /// </summary>
    /// <param name="domainObjectType">Тип доменного объекта.</param>
    /// <returns>
    ///     Коллекцию сохранённых описаний подписок с совпадающим типом доменного объекта.
    ///     Если совпадающих описаний нет, возращает пустую коллекцию.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент <paramref name="domainObjectType" /> равен null.
    /// </exception>
    public virtual IEnumerable<ISubscriptionMetadata> Get(Type domainObjectType)
    {
        if (domainObjectType == null)
        {
            throw new ArgumentNullException(nameof(domainObjectType));
        }

        var result = this.store[domainObjectType];
        return result;
    }
}
