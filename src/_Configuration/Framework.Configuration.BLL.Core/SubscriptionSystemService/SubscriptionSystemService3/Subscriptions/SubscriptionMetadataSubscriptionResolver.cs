using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
///     Компонент для поиска описаний подписок типа <see cref="ISubscriptionMetadata" />,
///     сохраненных в памяти и пребразования их в в экземпляры <see cref="Subscription" />.
///     Является декоратором экземпляра <see cref="SubscriptionResolver" />, выполняющего поиск подписок,
///     сохраненных в базе данных.
/// </summary>
/// <seealso cref="SubscriptionResolver" />
public sealed class SubscriptionMetadataSubscriptionResolver : SubscriptionResolver
{
    private readonly SubscriptionResolver wrappedResolver;
    private readonly SubscriptionMetadataStore metadataStore;
    private readonly SubscriptionMetadataMapper metadataMapper;
    private readonly ConfigurationContextFacade configurationContextFacade;

    /// <summary>
    ///     Создаёт экземпляр класса <see cref="SubscriptionMetadataSubscriptionResolver" />.
    /// </summary>
    /// <param name="resolver">Компонент, выполняющий поиск подписок.</param>
    /// <param name="metadataStore">Хранилище описаний подписок.</param>
    /// <param name="metadataMapper">Преобразователь экземпляров типа <see cref="ISubscriptionMetadata" /></param>
    /// <param name="configurationContextFacade"> Фасад контекста конфигурации.</param>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент
    ///     <paramref name="resolver" />
    ///     или
    ///     <paramref name="metadataStore" />
    ///     или
    ///     <paramref name="metadataMapper" />
    ///     или
    ///     <paramref name="configurationContextFacade"/>
    ///     равен null.
    /// </exception>
    public SubscriptionMetadataSubscriptionResolver(
            SubscriptionResolver resolver,
            SubscriptionMetadataStore metadataStore,
            SubscriptionMetadataMapper metadataMapper,
            ConfigurationContextFacade configurationContextFacade)
    {
        this.wrappedResolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        this.metadataStore = metadataStore ?? throw new ArgumentNullException(nameof(metadataStore));
        this.metadataMapper = metadataMapper ?? throw new ArgumentNullException(nameof(metadataMapper));
        this.configurationContextFacade = configurationContextFacade ?? throw new ArgumentNullException(nameof(configurationContextFacade));
    }

    /// <inheritdoc />
    public override IEnumerable<Subscription> Resolve<T>(DomainObjectVersions<T> versions)
    {
        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        var thisResult = this.GetSubscriptionsFromMetadata(versions.DomainObjectType);
        var otherResult = this.wrappedResolver.Resolve(versions);
        var result = thisResult.Concat(otherResult);

        return result;
    }

    /// <inheritdoc />
    public override Subscription Resolve<T>(
            string subscriptionCode,
            DomainObjectVersions<T> versions)
    {
        if (subscriptionCode == null)
        {
            throw new ArgumentNullException(nameof(subscriptionCode));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        return this
               .Resolve(versions)
               .FirstOrDefault(s => string.Equals(s.Code, subscriptionCode, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public override bool IsActiveSubscriptionForTypeExists(Type domainObjectType)
    {
        if (domainObjectType == null)
        {
            throw new ArgumentNullException(nameof(domainObjectType));
        }

        var exists = this.IsSubscriptionExists(domainObjectType)
                     || this.wrappedResolver.IsActiveSubscriptionForTypeExists(domainObjectType);

        return exists;
    }

    private bool IsSubscriptionExists(Type domainObjectType)
    {
        var logger = this.GetLogger();
        logger.LogDebug("Check subscription metadata for domain object type '{domainObjectType}' exists", domainObjectType);

        var result = this.metadataStore.Get(domainObjectType).Any();

        logger.LogDebug("Existing subscription metadata for domain object type '{domainObjectType}' {result}", domainObjectType, result ? "exists" : "not exists");

        return result;
    }

    private IEnumerable<Subscription> GetSubscriptionsFromMetadata(Type domainObjectType)
    {
        var logger = this.GetLogger();
        logger.LogDebug("Search subscriptions in metadata for domain object type '{domainObjectType}'", domainObjectType);

        var metadata = this.metadataStore.Get(domainObjectType).ToList();

        if (!metadata.Any())
        {
            return Enumerable.Empty<Subscription>();
        }

        var activeCodeFirstSubscriptionCodes = this.configurationContextFacade.GetActiveCodeFirstSubscriptionCodes();

        var result = metadata
                     .Where(m => activeCodeFirstSubscriptionCodes.Contains(m.Code))
                     .Select(m => this.metadataMapper.Map(m)).ToArray();

        logger.LogDebug("'{resultLength}' subscriptions has been found in metadata for domain object type '{domainObjectType}'", result.Length, domainObjectType);

        return result;
    }

    private ILogger<SubscriptionMetadataSubscriptionResolver> GetLogger() =>
        this.configurationContextFacade.ServiceProvider.GetRequiredService<ILogger<SubscriptionMetadataSubscriptionResolver>>();
}
