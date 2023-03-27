using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
///     Компонент для выполнения SubscriptionCondition лямбда-выражения при поиске подписок.
///     Является декоратором экземпляра <see cref="SubscriptionResolver" />, который действительно выполняет поиск.
/// </summary>
/// <seealso cref="SubscriptionResolver" />
public sealed class ConditionCheckSubscriptionsResolver<TBLLContext> : SubscriptionResolver
        where TBLLContext : class
{
    private readonly SubscriptionResolver wrappedResolver;
    private readonly ILogger logger;
    private readonly ConditionLambdaProcessor<TBLLContext> conditionProcessor;

    /// <summary>Создаёт экземпляр класса <see cref="ConditionCheckSubscriptionsResolver" />.</summary>
    /// <param name="resolver">Компонент, выполняющий поиск подписок.</param>
    /// <param name="lambdaProcessorFactory">Фабрика процессоров лямбда-выражений.</param>
    /// <param name="configurationContextFacade"> Фасад контекста конфигурации.</param>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент
    ///     resolver
    ///     или
    ///     configurationContextFacade
    ///     или
    ///     lambdaProcessorFactory равен null.
    /// </exception>
    public ConditionCheckSubscriptionsResolver(
            [NotNull] SubscriptionResolver resolver,
            [NotNull] LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory,
            [NotNull] ConfigurationContextFacade configurationContextFacade)
    {
        if (resolver == null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        if (lambdaProcessorFactory == null)
        {
            throw new ArgumentNullException(nameof(lambdaProcessorFactory));
        }

        if (configurationContextFacade == null)
        {
            throw new ArgumentNullException(nameof(configurationContextFacade));
        }

        this.wrappedResolver = resolver;
        this.logger = Log.Logger.ForContext(this.GetType());
        this.conditionProcessor = lambdaProcessorFactory.Create<ConditionLambdaProcessor<TBLLContext>>();
    }

    /// <summary>Выполняет поиск всех подписок, привязанных к конкретному типу доменного объекта.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Список подписок для конкретного типа доменного объекта.</returns>
    /// <exception cref="ArgumentNullException">versions</exception>
    /// <remarks>
    ///     Для каждой найденной подписки выполняется привязанное к подписке SubscriptionCondition ламбда выражение.
    ///     Если выражение возвращает <c>true</c>, подписка включается в результирующий список;
    ///     в противном случае подписка не включается в результирующий список.
    /// </remarks>
    public override IEnumerable<Subscription> Resolve<T>([NotNull] DomainObjectVersions<T> versions)
    {
        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        var result = new List<Subscription>();
        var subscriptions = this.wrappedResolver.Resolve(versions);

        foreach (var subscription in subscriptions)
        {
            this.TryProcessSubscriptionCondition(versions, subscription, result);
        }

        return result;
    }

    /// <inheritdoc />
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

        var result = this.wrappedResolver.Resolve(subscriptionCode, versions);
        return result;
    }

    /// <inheritdoc />
    public override bool IsActiveSubscriptionForTypeExists([NotNull] Type domainObjectType)
    {
        if (domainObjectType == null)
        {
            throw new ArgumentNullException(nameof(domainObjectType));
        }

        var result = this.wrappedResolver.IsActiveSubscriptionForTypeExists(domainObjectType);
        return result;
    }

    private void TryProcessSubscriptionCondition<T>(
            DomainObjectVersions<T> versions,
            Subscription subscription,
            ICollection<Subscription> result)
            where T : class
    {
        try
        {
            this.ProcessSubscriptionCondition(versions, subscription, result);
        }
        catch (Exception e)
        {
            this.logger.Error(e, "Subscription {subscription} condition lambda throws an exception: '{exception}'.", subscription, e);
        }
    }

    private void ProcessSubscriptionCondition<T>(
            DomainObjectVersions<T> versions,
            Subscription subscription,
            ICollection<Subscription> result)
            where T : class
    {
        if (this.conditionProcessor.Invoke(subscription, versions))
        {
            this.logger.Information("Subscription '{subscription}' satisfies condition lambda '{Condition}'.", subscription, subscription.Condition);

            result.Add(subscription);
        }
        else
        {
            this.logger.Information("Subscription '{subscription} 'not satisfies condition lambda '{Condition}' and excluded from process.", subscription, subscription.Condition);
        }
    }
}
