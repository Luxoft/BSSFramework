﻿using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Events;
using Framework.Notification;
using Framework.Persistent;

namespace Framework.Configuration.BLL;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : TargetSystemService<TBLLContext>, IPersistentTargetSystemService

        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    private readonly IEventOperationSender eventOperationSender;

    private readonly IRevisionSubscriptionSystemService subscriptionService;


    /// <summary>
    /// Создаёт экземпляр класса <see cref="TargetSystemService{TBLLContext, TPersistentDomainObjectBase}" />.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <param name="targetSystemContext">Контекст целевой системы.</param>
    /// <param name="targetSystem">Целевая система.</param>
    /// <param name="subscriptionMetadataStore">Хранилище описаний подписок.</param>
    public TargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystem targetSystem,
            IEventOperationSender eventOperationSender,
            SubscriptionMetadataStore subscriptionMetadataStore)
            : base(context, targetSystemContext, targetSystem, targetSystemContext.FromMaybe(() => new ArgumentNullException(nameof(targetSystemContext))).TypeResolver)
    {
        this.eventOperationSender = eventOperationSender;

        this.subscriptionService = this.GetSubscriptionService(subscriptionMetadataStore);
    }


    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public override ISubscriptionSystemService SubscriptionService => this.subscriptionService;

    public IRevisionSubscriptionSystemService GetSubscriptionService(IMessageSender<MessageTemplateNotification> subscriptionSender)
    {
        return this.subscriptionService;
    }

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

    public override bool IsAssignable(Type domainType)
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
