using Framework.Configuration.Core;
using Framework.Configuration.Domain;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
/// Компонент для поиска подписок, привязанных к типу доменного объекта.
/// </summary>
/// <seealso cref="SubscriptionResolver" />
public sealed class DomainObjectSubscriptionsResolver : SubscriptionResolver
{
    private readonly ConfigurationContextFacade configurationContextFacade;

    /// <summary>Создает экземпляр класса <see cref="DomainObjectSubscriptionsResolver"/>.</summary>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации.</param>
    /// <exception cref="ArgumentNullException">Аргумент configurationContextFacade равен null.</exception>
    public DomainObjectSubscriptionsResolver(ConfigurationContextFacade configurationContextFacade) =>
        this.configurationContextFacade = configurationContextFacade ?? throw new ArgumentNullException(nameof(configurationContextFacade));

    /// <inheritdoc/>
    public override IEnumerable<Subscription> Resolve<T>(DomainObjectVersions<T> versions)
    {
        yield break;
    }

    /// <inheritdoc/>
    public override bool IsActiveSubscriptionForTypeExists(Type domainObjectType) => false;

    /// <inheritdoc/>
    public override Subscription Resolve<T>(string subscriptionCode, DomainObjectVersions<T> versions)
    {
        if (subscriptionCode == null)
        {
            throw new ArgumentNullException(nameof(subscriptionCode));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        var logger = this.GetLogger();

        logger.LogDebug("Search active subscription for code '{subscriptionCode}' and domain object type '{DomainObjectType}'.", subscriptionCode, versions.DomainObjectType);

        var result = this
                     .Resolve(versions)
                     .FirstOrDefault(s => string.Equals(s.Code, subscriptionCode, StringComparison.OrdinalIgnoreCase));

        if (result == null)
        {
            var ex = new SubscriptionServicesException($"Subscription for code {subscriptionCode} not found.");
            logger.LogError(ex, "Error");
            throw ex;
        }

        logger.LogDebug("Active subscription for code '{subscriptionCode}' and domain object type '{DomainObjectType}' has been found.", subscriptionCode, versions.DomainObjectType);

        return result;
    }

    private ILogger<DomainObjectSubscriptionsResolver> GetLogger() =>
        this.configurationContextFacade.ServiceProvider.GetRequiredService<ILogger<DomainObjectSubscriptionsResolver>>();
}
