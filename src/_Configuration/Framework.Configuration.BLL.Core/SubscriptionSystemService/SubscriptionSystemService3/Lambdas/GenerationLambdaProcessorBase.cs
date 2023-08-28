using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;

/// <summary>
/// Базовый класс процессоров лямбда-выражений типа "Generation".
/// </summary>
/// <seealso cref="LambdaProcessor" />
public abstract class GenerationLambdaProcessorBase<TBLLContext> : LambdaProcessor<TBLLContext>
        where TBLLContext : class
{
    /// <summary>Создаёт экземпляр класса <see cref="GenerationLambdaProcessorBase"/>.</summary>
    /// <param name="bllContext">Контекст бизнес-логики.</param>
    protected GenerationLambdaProcessorBase(TBLLContext bllContext)
            : base(bllContext)
    {
    }

    /// <summary>Исполняет указанное в подписке ламбда-выражение типа "Generation".</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="subscription">Подписка.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Результат исполнения лямбда-выражения.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// subscription
    /// или
    /// versions равен null.
    /// </exception>
    public virtual IEnumerable<NotificationMessageGenerationInfo> Invoke<T>(
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

        var lambda = this.GetSubscriptionLambda(subscription);

        if (!DomainObjectCompliesLambdaRequiredMode(lambda, versions))
        {
            return Enumerable.Empty<NotificationMessageGenerationInfo>();
        }

        var result = this.TryInvoke(subscription, versions, this.InvokeInternal);

        return result;
    }

    /// <summary>Возвращает лямбда-выражения подписки.</summary>
    /// <param name="subscription">Подписка.</param>
    /// <returns>Лямбда-выражение подписки.</returns>
    protected abstract SubscriptionLambda GetSubscriptionLambda(Subscription subscription);

    /// <summary>Исполняет лямбда-выражение с типизированным контекстом бизнес-логики.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="subscription">Подписка.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Результат вызова лямбда-выражения.</returns>
    [UsedImplicitly]
    protected IEnumerable<NotificationMessageGenerationInfo> InvokeWithTypedContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        IEnumerable<NotificationMessageGenerationInfo> result;
        var funcValue = this.GetSubscriptionLambda(subscription).FuncValue;

        result = this.TryCast<IEnumerable<NotificationMessageGenerationInfo>>(funcValue(this.BllContext, versions));

        return result;
    }

    private IEnumerable<NotificationMessageGenerationInfo> InvokeInternal<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        return this.InvokeWithTypedContext(subscription, versions);
    }
}
