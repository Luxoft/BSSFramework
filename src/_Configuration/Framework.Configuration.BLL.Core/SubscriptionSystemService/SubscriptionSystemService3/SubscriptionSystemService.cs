using CommonFramework;

using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;

namespace Framework.Configuration.BLL.SubscriptionSystemService3;

/// <summary>
/// Предназначен для выполнения операций, связанных с подписками и изменяемыми доменными объектами.
/// </summary>
/// <seealso cref="Framework.Configuration.BLL.ISubscriptionSystemService" />
public class SubscriptionSystemService<TBLLContext> : ISubscriptionSystemService
        where TBLLContext : class
{
    private readonly SubscriptionServicesFactory<TBLLContext> servicesFactory;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="SubscriptionSystemService"/>.
    /// </summary>
    /// <param name="servicesFactory">Фабрика служб, используемая <see cref="SubscriptionSystemService"/>.</param>
    /// <exception cref="ArgumentNullException">Аргумент servicesFactory равен null.</exception>
    public SubscriptionSystemService(SubscriptionServicesFactory<TBLLContext> servicesFactory)
    {
        if (servicesFactory == null)
        {
            throw new ArgumentNullException(nameof(servicesFactory));
        }

        this.servicesFactory = servicesFactory;
    }

    /// <summary>
    /// Возвращает подписку, определяемую параметром subscriptionCode, и список получателей уведомлений по этой подписке.
    /// </summary>
    /// <param name="type">Тип доменного объекта.</param>
    /// <param name="prev">Предыдущая версия доменного объекта.</param>
    /// <param name="next">Текущая версия доменного объекта.</param>
    /// <param name="subscriptionCode">Код подписки.</param>
    /// <returns>Экземпляр <see cref="SubscriptionRecipientInfo"/>.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// type
    /// или
    /// prev
    /// или
    /// next
    /// или
    /// subscriptionCode равен null.
    /// </exception>
    public SubscriptionRecipientInfo GetRecipientsUntyped(
            Type type,
            object? prev,
            object? next,
            string subscriptionCode)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (subscriptionCode == null)
        {
            throw new ArgumentNullException(nameof(subscriptionCode));
        }

        var @delegate = (Func<object, object, string, SubscriptionRecipientInfo>)this.GetRecipientsTyped;
        var method = @delegate.CreateGenericMethod(type);
        var result = (SubscriptionRecipientInfo)method.Invoke(this, new[] { prev, next, subscriptionCode });

        return result;
    }

    /// <summary>
    /// Рассылает уведомления по подпискам для изменённого доменного объекта.
    /// </summary>
    /// <param name="prev">Предыдущая версия доменного объекта.</param>
    /// <param name="next">Текущая версия доменного объекта.</param>
    /// <param name="type">Тип доменного объекта.</param>
    /// <returns>Экземпляр <see cref="IList{ITryResult}"/>.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// prev
    /// или
    /// next
    /// или
    /// type равен null.
    /// </exception>
    public IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(
            object prev,
            object next,
            Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var @delegate = (Func<object, object, IList<ITryResult<Subscription>>>)this.ProcessChangedObjectTyped;
        var method = @delegate.CreateGenericMethod(type);
        var result = (IList<ITryResult<Subscription>>)method.Invoke(this, new[] { prev, next });

        return result;
    }

    /// <summary>
    /// Создаёт экземпляр службы уведомлений.
    /// </summary>
    /// <returns>Экземпляр <see cref="SubscriptionNotificationService"/>.</returns>
    protected SubscriptionNotificationService<TBLLContext> CreateNotificationService()
    {
        return this.servicesFactory.CreateNotificationService();
    }

    private SubscriptionRecipientInfo GetRecipientsTyped<TDomainObject>(
            TDomainObject? prev,
            TDomainObject? next,
            string subscriptionCode)
            where TDomainObject : class
    {
        var versions = new DomainObjectVersions<TDomainObject>(prev, next);
        var recipientService = this.CreateRecipientService();
        var result = recipientService.GetSubscriptionRecipientInfo(subscriptionCode, versions);

        return result;
    }

    private IList<ITryResult<Subscription>> ProcessChangedObjectTyped<TDomainObject>(
            TDomainObject prev,
            TDomainObject next)
            where TDomainObject : class
    {
        var versions = new DomainObjectVersions<TDomainObject>(prev, next);
        var notificationService = this.CreateNotificationService();
        var result = notificationService.NotifyDomainObjectChanged(versions);

        return result;
    }

    private RecipientService<TBLLContext> CreateRecipientService()
    {
        return this.servicesFactory.CreateRecipientService();
    }
}
