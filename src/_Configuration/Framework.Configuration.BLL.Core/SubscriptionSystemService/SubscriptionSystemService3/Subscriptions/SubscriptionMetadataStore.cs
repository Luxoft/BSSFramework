using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;
using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
///     Хранилище описаний подписок.
/// </summary>
public class SubscriptionMetadataStore
{
    private readonly ILookup<Type, ISubscriptionMetadata> store;

    /// <summary>
    ///     Создаёт экземпляр класса <see cref="SubscriptionMetadataStore" />.
    /// </summary>
    /// <param name="subscriptionMetadataFinder">Экземпляр компонента для поиска моделей подписок.</param>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент <paramref name="subscriptionMetadataFinder" /> равен null.
    /// </exception>
    public SubscriptionMetadataStore([NotNull] ISubscriptionMetadataFinder subscriptionMetadataFinder)
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
    public virtual IEnumerable<ISubscriptionMetadata> Get([NotNull] Type domainObjectType)
    {
        if (domainObjectType == null)
        {
            throw new ArgumentNullException(nameof(domainObjectType));
        }

        var result = this.store[domainObjectType];
        return result;
    }

    public virtual void RegisterCodeFirstSubscriptions(
            [NotNull] ICodeFirstSubscriptionBLL bll,
            [NotNull] IConfigurationBLLContext context)
    {
        if (bll == null)
        {
            throw new ArgumentNullException(nameof(bll));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var subscriptions = this.store
                                .SelectMany(g => g)
                                .Select(m => new CodeFirstSubscription (m.Code, context.GetDomainType(m.DomainObjectType, true)))
                                .ToArray();

        bll.Save(subscriptions);
    }
}
