using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Events;
using Framework.Persistent;

namespace Framework.Configuration.BLL;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<IConfigurationBLLContext>, ITargetSystemService

        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    private readonly IEventOperationSender eventOperationSender;

    private readonly Lazy<TargetSystem> lazyTargetSystem;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="TargetSystemService{TBLLContext, TPersistentDomainObjectBase}" />.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <param name="targetSystemContext">Контекст целевой системы.</param>
    /// <param name="subscriptionMetadataStore">Хранилище описаний подписок.</param>
    public TargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystemInfo<TPersistentDomainObjectBase> targetSystemInfo,
            IEventOperationSender eventOperationSender,
            SubscriptionMetadataStore subscriptionMetadataStore)
            : base(context)
    {
        this.Name = targetSystemInfo.Name;

        this.TargetSystemContext = targetSystemContext;
        this.eventOperationSender = eventOperationSender;

        this.SubscriptionService = this.GetSubscriptionService(subscriptionMetadataStore);

        this.lazyTargetSystem = LazyHelper.Create(() => context.Logics.TargetSystem.GetByName(this.Name, true));

        this.TypeResolver = this.TypeResolverS.OverrideInput((DomainType domainType) => domainType.FullTypeName).WithCache().WithLock();
    }

    public string Name { get; }

    public TBLLContext TargetSystemContext { get; }

    public ITypeResolver<string> TypeResolverS => this.TargetSystemContext.TypeResolver;

    public ITypeResolver<DomainType> TypeResolver { get; }

    public TargetSystem TargetSystem => this.lazyTargetSystem.Value;

    public IRevisionSubscriptionSystemService SubscriptionService { get; }

    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));

        var domainType = this.TypeResolver.Resolve(operation.DomainType);

        new Action<string, long?, Guid>(this.ForceEvent<TPersistentDomainObjectBase>)
            .CreateGenericMethod(domainType)
            .Invoke(this, new object[] { operation.Name, revision, domainObjectId });
    }

    private void ForceEvent<TDomainObject>(string operationName, long? revision, Guid domainObjectId)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        var bll = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

        var domainObject = revision == null ? bll.GetById(domainObjectId, true)
                                   : bll.GetObjectByRevision(domainObjectId, revision.Value);

        var domainObjectEvent = new EventOperation(operationName);

        this.eventOperationSender.Send(domainObject, domainObjectEvent);
    }

    public bool IsAssignable(Type domainType)
    {
        return typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType);
    }

    private IRevisionSubscriptionSystemService GetSubscriptionService(
            SubscriptionMetadataStore subscriptionMetadataStore)
    {
        var subscriptionServicesFactory = new SubscriptionServicesFactory<TBLLContext, TPersistentDomainObjectBase>(
            this.Context,
            this.TargetSystemContext.Logics.Default,
            this.TargetSystemContext,
            subscriptionMetadataStore);

        return new RevisionSubscriptionSystemService<TBLLContext, TPersistentDomainObjectBase>(subscriptionServicesFactory);
    }
}
