using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Templates;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Notification;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services;

/// <summary>
/// Служба рассылки уведомлений по подпискам и изменениям доменных объектов.
/// </summary>
public class SubscriptionNotificationService<TBLLContext>
        where TBLLContext : class
{
    private readonly SubscriptionResolver subscriptionsResolver;
    private readonly MessageTemplateFactory<TBLLContext> templateFactory;
    private readonly IMessageSender<MessageTemplateNotification> templateSender;

    private readonly ConfigurationContextFacade _configurationContextFacade;

    /// <summary>Создаёт экземпляр класса <see cref="SubscriptionNotificationService"/>.</summary>
    /// <param name="subscriptionsResolver">Компонент, выполняющий поиск подписок.</param>
    /// <param name="templateFactory">Фабрики шаблонов уведомлений.</param>
    /// <param name="sender">Отправитель шаблонов уведомлений.</param>
    /// <param name="configurationContextFacade">Фасад контекста конфигурации.</param>
    /// <exception cref="ArgumentNullException">Аргумент
    /// subscriptionsResolver
    /// или
    /// factory
    /// или
    /// filter
    /// или
    /// sender
    /// или configurationContextFacade
    /// равен null.
    /// </exception>
    public SubscriptionNotificationService(
            SubscriptionResolver subscriptionsResolver,
            MessageTemplateFactory<TBLLContext> templateFactory,
            IMessageSender<MessageTemplateNotification> sender,
            ConfigurationContextFacade configurationContextFacade)
    {
        this.subscriptionsResolver = subscriptionsResolver ?? throw new ArgumentNullException(nameof(subscriptionsResolver));
        this.templateFactory = templateFactory ?? throw new ArgumentNullException(nameof(templateFactory));
        this.templateSender = sender ?? throw new ArgumentNullException(nameof(sender));
        this._configurationContextFacade = configurationContextFacade ?? throw new ArgumentNullException(nameof(configurationContextFacade));
    }

    /// <summary>Выполняет рассылку уведомлений об изменениях доменного объекта.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>
    ///     Экземпляр <see cref="IList{ITryResult}"/>, содержащий ошибки, произошедшие во время исполнения.
    ///     Если ошибок не было, возвращается пустой список.
    /// </returns>
    /// <exception cref="ArgumentNullException">Аргумент versions равен null.</exception>
    public virtual IList<ITryResult<Subscription>> NotifyDomainObjectChanged<T>(
            DomainObjectVersions<T> versions)
            where T : class
    {
        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        var result = this.NotifyInternal(versions);
        return result.ToList();
    }

    /// <summary>Выполняет рассылку уведомлений об изменениях доменного объекта по конкретной подписке.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="subscription">Подписка по которой будут рассылаться уведомления.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <exception cref="ArgumentNullException">Аргумент subscription или versions равен null.</exception>
    public virtual void NotifyDomainObjectChanged<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        if (subscription == null)
        {
            throw new ArgumentNullException(nameof(subscription));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        this.NotifyInternal(subscription, versions);
    }

    private IEnumerable<ITryResult<Subscription>> NotifyInternal<T>(DomainObjectVersions<T> versions)
            where T : class
    {
        var logger = this.GetLogger();
        logger.LogDebug("Send notifications for domain object '{versions}' changes.", versions);

        var findSubscriptionsResult = this.TryFindSubscriptions(versions);
        var createTemplatesResult = this.TryCreateTemplates(findSubscriptionsResult, versions);
        var sendTemplatesResult = this.TrySendTemplates(createTemplatesResult);

        var result = this.GetFaults(findSubscriptionsResult)
                         .Union(this.GetFaults(createTemplatesResult))
                         .Union(this.GetFaults(sendTemplatesResult))
                         .ToList();

        var logMessage = "Notifications for domain object '{versions}' changes has been sent. '{Count}' errors occured.";

        if (result.Count == 0)
        {
            logger.LogDebug(logMessage, versions, result.Count);
        }
        else
        {
            logger.LogWarning(logMessage, versions, result.Count);
        }

        return result;
    }

    private void NotifyInternal<T>(Subscription subscription, DomainObjectVersions<T> versions)
            where T : class
    {
        var logger = this.GetLogger();
        logger.LogDebug("Send notifications for subscription '{subscription}' and domain object '{versions}' changes.", subscription, versions);

        var templates = this.CreateTemplates(subscription, versions);
        this.SendTemplates(templates);

        logger.LogDebug("Notifications for subscription '{subscription}' and domain object '{versions}' changes has been sent successfully.", subscription, versions);
    }

    private ITryResult<IEnumerable<Subscription>> TryFindSubscriptions<T>(DomainObjectVersions<T> versions)
            where T : class
    {
        var result = TryResult.Catch(() => this.subscriptionsResolver.Resolve(versions));
        return result;
    }

    private IEnumerable<ITryResult<IEnumerable<MessageTemplateNotification>>> TryCreateTemplates<T>(
            ITryResult<IEnumerable<Subscription>> tryResult,
            DomainObjectVersions<T> versions)
            where T : class
    {
        if (tryResult.IsFault())
        {
            return Enumerable.Empty<ITryResult<IEnumerable<MessageTemplateNotification>>>();
        }

        var subscriptions = tryResult.GetValue();
        var result = this.TryCreateTemplates(subscriptions, versions);
        return result.ToList();
    }

    private IEnumerable<ITryResult<IEnumerable<MessageTemplateNotification>>> TryCreateTemplates<T>(
            IEnumerable<Subscription> subscriptions,
            DomainObjectVersions<T> versions)
            where T : class
    {
        try
        {
            var templates = this.templateFactory.Create(subscriptions.ToList(), versions);
            var groups = templates.GroupBy(t => t.Subscription);
            var results = groups.Select(g => TryResult.Return(g.ToList()));
            return results;
        }
        catch (AggregateException ex)
        {
            var faults = ex.InnerExceptions.Select(TryResult.CreateFault<IEnumerable<MessageTemplateNotification>>);
            return faults;
        }
        catch (Exception ex)
        {
            var fault = TryResult.CreateFault<IEnumerable<MessageTemplateNotification>>(ex);
            return new[] { fault };
        }
    }

    private IEnumerable<MessageTemplateNotification> CreateTemplates<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        var result = this.templateFactory.Create(new List<Subscription> { subscription }, versions);
        return result;
    }

    private IEnumerable<ITryResult<MessageTemplateNotification>> TrySendTemplates(
            IEnumerable<ITryResult<IEnumerable<MessageTemplateNotification>>> tryResult)
    {
        var successResults = tryResult.Where(r => r.IsSuccess());

        if (!successResults.Any())
        {
            return Enumerable.Empty<ITryResult<MessageTemplateNotification>>();
        }

        var templates = successResults.SelectMany(r => r.GetValue());
        var result = templates.Select(t => TryResult.Catch(() => this.SendTemplate(t)));

        return result.ToList();
    }

    private void SendTemplates(IEnumerable<MessageTemplateNotification> templates)
    {
        foreach (var template in templates)
        {
            this.SendTemplate(template);
        }
    }

    private MessageTemplateNotification SendTemplate(MessageTemplateNotification template)
    {
        this.templateSender.Send(template);
        return template;
    }

    private IEnumerable<ITryResult<Subscription>> GetFaults<T>(IEnumerable<ITryResult<T>> tryResults)
    {
        var results = tryResults.GetErrors().Select(TryResult.CreateFault<Subscription>);
        return results.ToList();
    }

    private IEnumerable<ITryResult<Subscription>> GetFaults<T>(ITryResult<T> tryResult)
    {
        return this.GetFaults(new[] { tryResult });
    }

    private ILogger<SubscriptionNotificationService<TBLLContext>> GetLogger() =>
        this._configurationContextFacade.ServiceProvider.GetRequiredService<ILogger<SubscriptionNotificationService<TBLLContext>>>();
}
