using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
/// Компонент для поиска подписок, привязанных к типу доменного объекта.
/// </summary>
/// <seealso cref="SubscriptionResolver" />
public sealed class DomainObjectSubscriptionsResolver : SubscriptionResolver
{
    private readonly ConfigurationContextFacade configurationContextFacade;
    private readonly ILogger logger;

    /// <summary>Создает экземпляр класса <see cref="DomainObjectSubscriptionsResolver"/>.</summary>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации.</param>
    /// <exception cref="ArgumentNullException">Аргумент configurationContextFacade равен null.</exception>
    public DomainObjectSubscriptionsResolver([NotNull] ConfigurationContextFacade configurationContextFacade)
    {
        if (configurationContextFacade == null)
        {
            throw new ArgumentNullException(nameof(configurationContextFacade));
        }

        this.configurationContextFacade = configurationContextFacade;

        this.logger = Log.ForContext(this.GetType());
    }

    /// <inheritdoc/>
    public override IEnumerable<Subscription> Resolve<T>([NotNull] DomainObjectVersions<T> versions)
    {
        yield break;
    }

    /// <inheritdoc/>
    public override bool IsActiveSubscriptionForTypeExists([NotNull] Type domainObjectType)
    {
        return false;
    }

    /// <inheritdoc/>
    public override Subscription Resolve<T>(
            [NotNull] string subscriptionCode,
            [NotNull] DomainObjectVersions<T> versions)
    {
        if (subscriptionCode == null)
        {
            throw new ArgumentNullException(nameof(subscriptionCode));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        this.logger.Verbose("Search active subscription for code '{subscriptionCode}' and domain object type '{DomainObjectType}'.", subscriptionCode, versions.DomainObjectType);

        var result = this
                     .Resolve(versions)
                     .FirstOrDefault(s => string.Equals(s.Code, subscriptionCode, StringComparison.OrdinalIgnoreCase));

        if (result == null)
        {
            var ex = new SubscriptionServicesException($"Subscription for code {subscriptionCode} not found.");
            this.logger.Error(ex, "Error");
            throw ex;
        }

        this.logger.Verbose("Active subscription for code '{subscriptionCode}' and domain object type '{DomainObjectType}' has been found.", subscriptionCode, versions.DomainObjectType);

        return result;
    }
}
