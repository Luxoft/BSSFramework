using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;

using Serilog;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services;

/// <summary>
/// Служба поиска подписки и информации о получателях уведомлений по подписке.
/// </summary>
public class RecipientService<TBLLContext>
        where TBLLContext : class
{
    private readonly SubscriptionResolver subscriptionResolver;
    private readonly RecipientsResolver<TBLLContext> recipientsResolver;
    private readonly ILogger logger;

    /// <summary>Создаёт экземпляр класса <see cref="RecipientService"/>.</summary>
    /// <param name="subscriptionResolver">Компонент, выполняющий поиск подписок.</param>
    /// <param name="recipientsResolver">Компонент, выполняющий поиск получателей уведомлений по подписке.</param>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации.</param>
    /// <exception cref="ArgumentNullException">Аргумент
    /// subscriptionResolver
    /// или
    /// recipientsResolver
    /// или
    /// loggerFactory
    /// равен null.
    /// </exception>
    public RecipientService(
            SubscriptionResolver subscriptionResolver,
            RecipientsResolver<TBLLContext> recipientsResolver,
            ConfigurationContextFacade configurationContextFacade)
    {
        if (subscriptionResolver == null)
        {
            throw new ArgumentNullException(nameof(subscriptionResolver));
        }

        if (recipientsResolver == null)
        {
            throw new ArgumentNullException(nameof(recipientsResolver));
        }

        if (configurationContextFacade == null)
        {
            throw new ArgumentNullException(nameof(configurationContextFacade));
        }

        this.subscriptionResolver = subscriptionResolver;
        this.recipientsResolver = recipientsResolver;

        this.logger = Log.Logger.ForContext(this.GetType());
    }

    /// <summary>Выполняет поиск подписки и получателей уведомлений по подписке.</summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    /// <param name="subscriptionCode">Код подписки для поиска.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Экземпляр <see cref="SubscriptionRecipientInfo"/>.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// subscriptionCode
    /// или
    /// versions равен null.
    /// </exception>
    public virtual SubscriptionRecipientInfo GetSubscriptionRecipientInfo<T>(
            string subscriptionCode,
            DomainObjectVersions<T> versions)
            where T : class
    {
        if (subscriptionCode == null)
        {
            throw new ArgumentNullException(nameof(subscriptionCode));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        this.logger.Information("Get recipients for subscription code '{subscriptionCode}' and domain object type '{DomainObjectType}'.", subscriptionCode, versions.DomainObjectType);

        var subscription = this.GetSubscription(subscriptionCode, versions);

        if (subscription == null)
        {
            this.logger.Information("Active subscription for code '{subscriptionCode}' and domain object type '{DomainObjectType}' not found.", subscriptionCode, versions.DomainObjectType);
            return null;
        }

        var recipients = this.GetRecipients(versions, subscription);

        this.logger.Information("For subscription '{subscription}' '{recipients.Count}' recipients has been found. Recipients: {recipients}.", subscription, recipients.Join(", "));

        var result = new SubscriptionRecipientInfo
                     {
                             Subscription = subscription,
                             Recipients = recipients
                     };

        return result;
    }

    private List<string> GetRecipients<T>(DomainObjectVersions<T> versions, Subscription subscription)
            where T : class
    {
        var resolverResults = this.recipientsResolver.Resolve(subscription, versions);

        var recipients = resolverResults
                         .Select(r => r.RecipientsBag)
                         .SelectMany(b => b.To)
                         .Select(r => r.Email)
                         .ToList();
        return recipients;
    }

    private Subscription GetSubscription<T>(string subscriptionCode, DomainObjectVersions<T> versions)
            where T : class
    {
        try
        {
            var subscription = this.subscriptionResolver.Resolve(subscriptionCode, versions);
            return subscription;
        }
        catch (SubscriptionServicesException ex)
        {
            this.logger.Error(ex, "GetSubscription");
            return null;
        }
    }
}
