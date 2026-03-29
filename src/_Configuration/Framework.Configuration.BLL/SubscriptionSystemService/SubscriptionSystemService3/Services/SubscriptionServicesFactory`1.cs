using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Templates;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services;

/// <summary>
/// Фабрика служб, используемых системой подписок, уведомлений и версий доменных объектов.
/// </summary>
public class SubscriptionServicesFactory<TBLLContext>
        where TBLLContext : class
{
    private readonly TBLLContext bllContext;
    private readonly IConfigurationBLLContext configurationContext;
    private readonly SubscriptionMetadataStore subscriptionMetadataStore;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="SubscriptionServicesFactory" />.
    /// </summary>
    /// <param name="configurationContext">Контекст конфигурации.</param>
    /// <param name="bllContext">Контекст текущей бизнес-логики.</param>
    /// <param name="subscriptionMetadataStore">Экземпляр хранилища моделей подписок.</param>
    /// <exception>Аргумент
    /// configurationContext
    /// or
    /// bllContext
    /// or
    /// subscriptionMetadataStore
    /// равен null
    ///     <cref>System.ArgumentNullException</cref>
    /// </exception>
    public SubscriptionServicesFactory(
            IConfigurationBLLContext configurationContext,
            TBLLContext bllContext,
            SubscriptionMetadataStore subscriptionMetadataStore)
    {
        this.configurationContext = configurationContext ?? throw new ArgumentNullException(nameof(configurationContext));
        this.bllContext = bllContext ?? throw new ArgumentNullException(nameof(bllContext));
        this.subscriptionMetadataStore = subscriptionMetadataStore ?? throw new ArgumentNullException(nameof(subscriptionMetadataStore));
    }
    private LambdaProcessorFactory<TBLLContext> CreateLambdaProcessorFactory() =>
            new LambdaProcessorFactory<TBLLContext>(this.bllContext);

    protected ConditionCheckSubscriptionsResolver<TBLLContext> CreateSubscriptionsResolver()
    {
        var contextFacade = this.CreateConfigurationContextFacade();

        return new ConditionCheckSubscriptionsResolver<TBLLContext>(
                                                                    new SubscriptionMetadataSubscriptionResolver(
                                                                     new DomainObjectSubscriptionsResolver(contextFacade),
                                                                     this.subscriptionMetadataStore,
                                                                     new SubscriptionMetadataMapper(contextFacade),
                                                                     contextFacade),
                                                                    this.CreateLambdaProcessorFactory(),
                                                                    contextFacade);
    }

    private RecipientsResolver<TBLLContext> CreateRecipientsResolver()
    {
        var contextFacade = this.CreateConfigurationContextFacade();
        return new RecipientsResolver<TBLLContext>(
                                                   new GenerationRecipientsResolver<TBLLContext>(this.CreateLambdaProcessorFactory()),
                                                   new ByRolesRecipientsResolver<TBLLContext>(contextFacade, this.CreateLambdaProcessorFactory()),
                                                   contextFacade);
    }

    /// <summary>
    /// Createa attachment resolver that resoleves attach lambdas into real attachments
    /// </summary>
    /// <returns>New resolver instance</returns>
    private AttachmentsResolver<TBLLContext> CreateAttachmentsResolver() => new AttachmentsResolver<TBLLContext>(this.CreateLambdaProcessorFactory());

    /// <summary>Создаёт экземпляр службы рассылки уведомлений.</summary>
    /// <returns>Экземпляр службы рассылки уведомлений.</returns>
    public virtual SubscriptionNotificationService<TBLLContext> CreateNotificationService()
    {
        var contextFacade = this.CreateConfigurationContextFacade();
        var result = new SubscriptionNotificationService<TBLLContext>(
                                                                      this.CreateSubscriptionsResolver(),
                                                                      new MessageTemplateFactory<TBLLContext>(this.CreateRecipientsResolver(), this.CreateAttachmentsResolver(), new ExcessTemplatesFilter(), contextFacade),
                                                                      this.configurationContext.SubscriptionSender,
                                                                      contextFacade);

        return result;
    }

    /// <summary>Создаёт экземпляр службы поиска подписки и информации о получателях уведомлений по подписке.</summary>
    /// <returns>Экземпляр службы поиска подписки и информации о получателях уведомлений по подписке.</returns>
    public virtual RecipientService<TBLLContext> CreateRecipientService()
    {
        var contextFacade = this.CreateConfigurationContextFacade();
        var result = new RecipientService<TBLLContext>(
                                                       new DomainObjectSubscriptionsResolver(contextFacade),
                                                       this.CreateRecipientsResolver(),
                                                       contextFacade);

        return result;
    }

    /// <summary>Создаёт экземпляр процессора лямбда-выражений.</summary>
    /// <typeparam name="TProcessor">Тип создаваемого процессора лямбда-выражений.</typeparam>
    /// <returns>Экземпляр процессора лямбда-выражений.</returns>
    public virtual TProcessor CreateLambdaProcessor<TProcessor>()
            where TProcessor : LambdaProcessor<TBLLContext>
    {
        var result = this.CreateLambdaProcessorFactory().Create<TProcessor>();
        return result;
    }

    /// <summary>Создаёт экземпляр службы поиска версий доменного объекта.</summary>
    /// <param name="domainObjectType">Тип доменного объекта.</param>
    /// <returns>Экземпляр службы поиска версий доменного объекта.</returns>
    /// <exception cref="NotSupportedException">В этом типе метод не поддерживается.</exception>
    public virtual object CreateRevisionService(Type domainObjectType)
    {
        var message = "This method is not supported on instance of SubscriptionServicesFactory. Use SubscriptionServicesFactory<> instead.";
        throw new NotSupportedException(message);
    }

    /// <summary>
    /// Создаёт экземпляр фасада контекста конфигурации.
    /// </summary>
    /// <returns>Экземпляр фасада контекста конфигурации.</returns>
    public virtual ConfigurationContextFacade CreateConfigurationContextFacade()
    {
        return new ConfigurationContextFacade(this.configurationContext);
    }
}
