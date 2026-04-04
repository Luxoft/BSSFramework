using CommonFramework;

using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.Configuration.BLL.TargetSystemService;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<IConfigurationBLLContext>, ITargetSystemService

    where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
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
        IEventOperationSender eventOperationSender)
        : base(context)
    {
        this.Name = targetSystemInfo.Name;

        this.TargetSystemContext = targetSystemContext;
        this.eventOperationSender = eventOperationSender;

        this.lazyTargetSystem = LazyHelper.Create(() => context.Logics.TargetSystem.GetByName(this.Name, true));

        this.TypeResolver = this.TypeResolverS.OverrideInput((DomainType domainType) => domainType.FullTypeName);
    }

    public string Name { get; }

    public TBLLContext TargetSystemContext { get; }

    public Type BLLContextType { get; } = typeof(TBLLContext);

    public ITypeResolver<string> TypeResolverS => this.TargetSystemContext.TypeResolver;

    public ITypeResolver<DomainType> TypeResolver { get; }

    public TargetSystem TargetSystem => this.lazyTargetSystem.Value;

    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));

        var domainType = this.TypeResolver.TryResolve(operation.DomainType);

        new Action<string, long?, Guid>(this.ForceEvent<TPersistentDomainObjectBase>)
            .CreateGenericMethod(domainType)
            .Invoke(this, [operation.Name, revision, domainObjectId]);
    }

    private void ForceEvent<TDomainObject>(string operationName, long? revision, Guid domainObjectId)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var bll = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

        var domainObject = revision == null
                               ? bll.GetById(domainObjectId, true)
                               : bll.GetObjectByRevision(domainObjectId, revision.Value);

        var domainObjectEvent = new EventOperation(operationName);

        this.eventOperationSender.Send(domainObject, domainObjectEvent, CancellationToken.None).GetAwaiter().GetResult();
    }

    public bool IsAssignable(Type domainType) => typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType);
}
