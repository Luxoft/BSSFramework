using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Events;
using Framework.Notification;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : TargetSystemService<TBLLContext>, IPersistentTargetSystemService

        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>, IBLLOperationEventContext<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    private readonly IEnumerable<IManualEventDALListener<TPersistentDomainObjectBase>> eventDalListeners;

    private readonly IRevisionSubscriptionSystemService subscriptionService;


    /// <summary>
    /// Создаёт экземпляр класса <see cref="TargetSystemService{TBLLContext, TPersistentDomainObjectBase}" />.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <param name="targetSystemContext">Контекст целевой системы.</param>
    /// <param name="targetSystem">Целевая система.</param>
    /// <param name="subscriptionMetadataStore">Хранилище описаний подписок.</param>
    /// <param name="eventDalListeners">DAL-подписчики для пробразсывания евентов</param>
    public TargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystem targetSystem,
            IEnumerable<IManualEventDALListener<TPersistentDomainObjectBase>> eventDalListeners,
            SubscriptionMetadataStore subscriptionMetadataStore = null)
            : base(context, targetSystemContext, targetSystem, targetSystemContext.FromMaybe(() => new ArgumentNullException(nameof(targetSystemContext))).TypeResolver)
    {
        this.eventDalListeners = (eventDalListeners ?? throw new ArgumentNullException(nameof(eventDalListeners)));

        this.subscriptionService = this.GetSubscriptionService(subscriptionMetadataStore ?? new SubscriptionMetadataStore(new SubscriptionMetadataFinder()));
    }


    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public override ISubscriptionSystemService SubscriptionService => this.subscriptionService;

    public IRevisionSubscriptionSystemService GetSubscriptionService(IMessageSender<MessageTemplateNotification> subscriptionSender)
    {
        return this.subscriptionService;
    }

    public void ForceEvent([NotNull] DomainTypeEventOperation operation, long? revision, Guid domainObjectId)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));

        var domainType = this.TypeResolver.Resolve(operation.DomainType);

        var operationType = domainType.GetEventOperationType() ?? typeof(BLLBaseOperation);

        new Action<string, long?, Guid>(this.ForceEvent<TPersistentDomainObjectBase, TypeCode>)
            .CreateGenericMethod(domainType, operationType)
            .Invoke(this, new object[] { operation.Name, revision, domainObjectId });
    }

    private void ForceEvent<TDomainObject, TOperation>(string operationName, long? revision, Guid domainObjectId)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
    {
        var bll = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

        var domainObject = revision == null ? bll.GetById(domainObjectId, true)
                                   : bll.GetObjectByRevision(domainObjectId, revision.Value);

        var operation = EnumHelper.Parse<TOperation>(operationName);

        var listener = this.TargetSystemContext.OperationSenders.GetEventSender<TDomainObject, TOperation>();

        listener.SendEvent(domainObject, operation);

        operation.ToOperationMaybe<TOperation, EventOperation>().Match(
                                                                       eventOperation =>
                                                                               this.eventDalListeners.Foreach(dalListener => dalListener.GetForceEventContainer<TDomainObject>().SendEvent(domainObject, eventOperation)));
    }

    public override bool IsAssignable(Type domainType)
    {
        return typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType);
    }

    private IRevisionSubscriptionSystemService GetSubscriptionService(
            SubscriptionMetadataStore subscriptionMetadataStore)
    {
        if (subscriptionMetadataStore == null)
        {
            throw new InvalidOperationException("SubscriptionMetadataStore instance can not be null for use new subscription services.");
        }

        var subscriptionServicesFactory = new SubscriptionServicesFactory<TBLLContext, TPersistentDomainObjectBase>(
         this.Context,
         this.TargetSystemContext.Logics.Default,
         this.TargetSystemContext,
         subscriptionMetadataStore);

        return new RevisionSubscriptionSystemService<TBLLContext, TPersistentDomainObjectBase>(subscriptionServicesFactory);
    }

    IRevisionSubscriptionSystemService IPersistentTargetSystemService.SubscriptionService
    {
        get { return this.subscriptionService; }
    }
}



public abstract class TargetSystemService<TBLLContext> : BLLContextContainer<IConfigurationBLLContext>, ITargetSystemService<TBLLContext>

        where TBLLContext : class
{
    protected TargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystem targetSystem,
            ITypeResolver<string> typeResolver)
            : base(context)
    {
        this.TargetSystemContext = targetSystemContext ?? throw new ArgumentNullException(nameof(targetSystemContext));
        this.TargetSystem = targetSystem ?? throw new ArgumentNullException(nameof(targetSystem));

        this.TypeResolverS = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        this.TypeResolver = typeResolver.OverrideInput((DomainType domainType) => domainType.FullTypeName).WithCache().WithLock();
    }



    public string Name
    {
        get { return this.TargetSystem.Name; }
    }

    public TBLLContext TargetSystemContext { get; private set; }

    public TargetSystem TargetSystem { get; private set; }

    public ITypeResolver<string> TypeResolverS { get; private set; }

    public ITypeResolver<DomainType> TypeResolver { get; private set; }

    public abstract ISubscriptionSystemService SubscriptionService { get; }

    public abstract bool IsAssignable(Type domainType);


    object ITargetSystemService.TargetSystemContext
    {
        get { return this.TargetSystemContext; }
    }

    Type ITargetSystemService.TargetSystemContextType
    {
        get { return typeof(TBLLContext); }
    }
}
