using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

/// <summary>
/// Компонент для поиска получателей уведомлений, указанных в подписке как набор ролей.
/// </summary>
public class ByRolesRecipientsResolver<TBLLContext>
        where TBLLContext : class
{
    private static readonly Dictionary<SubscriptionSourceMode, Func<ConfigurationContextFacade, LambdaProcessorFactory<TBLLContext>, ByRolesRecipientsResolverBase<TBLLContext>>>
            Resolvers =
                    new Dictionary<SubscriptionSourceMode, Func<ConfigurationContextFacade, LambdaProcessorFactory<TBLLContext>, ByRolesRecipientsResolverBase<TBLLContext>>>
                    {
                            { SubscriptionSourceMode.Dynamic, (cf, pf) => new ByRolesRecipientsResolverDynamic<TBLLContext>(cf, pf) },
                            { SubscriptionSourceMode.Typed, (cf, pf) => new ByRolesRecipientsResolverTyped<TBLLContext>(cf, pf) },
                            { SubscriptionSourceMode.NonContext, (cf, pf) => new ByRolesRecipientsResolverNonContext<TBLLContext>(cf, pf) },
                            { SubscriptionSourceMode.Invalid, (cf, pf) => new ByRolesRecipientsResolverBase<TBLLContext>(cf, pf) }
                    };

    private readonly ConfigurationContextFacade configurationContextFacade;
    private readonly LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory;

    /// <summary>Создаёт экземпляр класса <see cref="ByRolesRecipientsResolver"/>.</summary>
    /// <param name="configurationContextFacade">Контекст конфигурации.</param>
    /// <param name="lambdaProcessorFactory">Фабрика процессоров лямбда-выражений.</param>
    /// <exception cref="ArgumentNullException">Аргумент
    /// configurationContextFacade
    /// или
    /// lambdaProcessorFactory равен null.
    /// </exception>
    public ByRolesRecipientsResolver(
            [NotNull] ConfigurationContextFacade configurationContextFacade,
            [NotNull] LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    {
        this.configurationContextFacade = configurationContextFacade ?? throw new ArgumentNullException(nameof(configurationContextFacade));
        this.lambdaProcessorFactory = lambdaProcessorFactory ?? throw new ArgumentNullException(nameof(lambdaProcessorFactory));
    }

    /// <summary>Выполняет поиск получателей уведомлений по подписке.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="subscription">Подписка.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Список найденных получателей уведомлений.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// subscription
    /// или
    /// versions равен null.
    /// </exception>
    public virtual RecipientCollection Resolve<T>(
            [NotNull] Subscription subscription,
            [NotNull] DomainObjectVersions<T> versions)
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

        var resolver = this.CreateResolver(subscription.SourceMode);
        var result = resolver.Resolve(subscription, versions);

        return result;
    }

    private ByRolesRecipientsResolverBase<TBLLContext> CreateResolver(SubscriptionSourceMode mode)
    {
        var createResolver = Resolvers[mode];
        var result = createResolver(this.configurationContextFacade, this.lambdaProcessorFactory);

        return result;
    }
}
