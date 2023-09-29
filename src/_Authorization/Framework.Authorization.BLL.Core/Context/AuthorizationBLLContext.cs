using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;

using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.SecuritySystem;


using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext
{
    private readonly IAuthorizationBLLFactoryContainer logics;

    private readonly Lazy<Principal> lazyCurrentPrincipal;

    private readonly Lazy<Settings> lazySettings;

    private readonly IDictionaryCache<string, EntityType> entityTypeByNameCache;

    private readonly IDictionaryCache<Guid, EntityType> entityTypeByIdCache;

    private readonly ISecurityProvider<Operation> operationSecurityProvider;

    public AuthorizationBLLContext(
            IServiceProvider serviceProvider,
            IOperationEventSenderContainer<PersistentDomainObjectBase> operationSenders,
            ITrackingService<PersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            IAuthorizationValidator validator,
            IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            IDateTimeService dateTimeService,
            IConfigurationBLLContext configuration,
            IRootSecurityService<PersistentDomainObjectBase> securityService,
            IAuthorizationBLLFactoryContainer logics,
            IAuthorizationExternalSource externalSource,
            ISecurityTypeResolverContainer securityTypeResolverContainer,
            INotificationPrincipalExtractor notificationPrincipalExtractor,
            IAuthorizationBLLContextSettings settings,
            IAuthorizationSystem<Guid> authorizationSystem,
            IRunAsManager runAsManager,
            IAvailablePermissionSource availablePermissionSource,
            ISecurityOperationParser securityOperationParser,
            IAvailableSecurityOperationSource availableSecurityOperationSource)
            : base(
                   serviceProvider,
                   operationSenders,
                   trackingService,
                   accessDeniedExceptionService,
                   standartExpressionBuilder,
                   validator,
                   hierarchicalObjectExpanderFactory,
                   fetchService)
    {
        this.DateTimeService = dateTimeService;
        this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        this.logics = logics ?? throw new ArgumentNullException(nameof(logics));
        this.AvailablePermissionSource = availablePermissionSource;
        this.SecurityOperationParser = securityOperationParser;
        this.AvailableSecurityOperationSource = availableSecurityOperationSource;
        this.NotificationPrincipalExtractor = notificationPrincipalExtractor;
        this.AuthorizationSystem = authorizationSystem;
        this.RunAsManager = runAsManager;

        this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));
        this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        this.lazyCurrentPrincipal = LazyHelper.Create(() => this.Logics.Principal.GetCurrent());


        this.SecurityTypeResolver = securityTypeResolverContainer.SecurityTypeResolver.OverrideInput((EntityType entityType) => entityType.Name);

        this.lazySettings = LazyHelper.Create(() => this.Logics.Default.Create<Setting>().GetFullList().ToSettings());

        this.entityTypeByNameCache = new DictionaryCache<string, EntityType>(
                                                                             domainTypeName => this.Logics.EntityType.GetByName(domainTypeName, true),
                                                                             StringComparer.CurrentCultureIgnoreCase)
                .WithLock();

        this.entityTypeByIdCache = new DictionaryCache<Guid, EntityType>(
                                                                         domainTypeId => this.Logics.EntityType.GetById(domainTypeId, true))
                .WithLock();

        this.operationSecurityProvider = new OperationSecurityProvider(this);

        this.TypeResolver = settings.TypeResolver;
    }

    public IConfigurationBLLContext Configuration { get; }

    public ITypeResolver<string> TypeResolver { get; }

    public ITypeResolver<EntityType> SecurityTypeResolver { get; }

    public INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    public IAuthorizationSystem<Guid> AuthorizationSystem { get; }

    public IRunAsManager RunAsManager { get; }

    public IAvailablePermissionSource AvailablePermissionSource { get; }

    public IAvailableSecurityOperationSource AvailableSecurityOperationSource { get; }

    public ISecurityOperationParser SecurityOperationParser { get; }

    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; }

    public Settings Settings => this.lazySettings.Value;

    public override IAuthorizationBLLFactoryContainer Logics => this.logics;

    public IAuthorizationExternalSource ExternalSource { get; }

    public Principal CurrentPrincipal => this.lazyCurrentPrincipal.Value;


    public IDateTimeService DateTimeService { get; }


    public EntityType GetEntityType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.GetEntityType(type.Name);
    }

    public EntityType GetEntityType(string domainTypeName)
    {
        if (domainTypeName == null) throw new ArgumentNullException(nameof(domainTypeName));

        return this.entityTypeByNameCache[domainTypeName];
    }

    public EntityType GetEntityType(Guid domainTypeId)
    {
        if (domainTypeId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainTypeId));

        return this.entityTypeByIdCache[domainTypeId];
    }

    public ISecurityProvider<TDomainObject> GetPrincipalSecurityProvider<TDomainObject>(
            Expression<Func<TDomainObject, Principal>> principalSecurityPath)
            where TDomainObject : PersistentDomainObjectBase
    {
        if (principalSecurityPath == null) throw new ArgumentNullException(nameof(principalSecurityPath));

        return new PrincipalSecurityProvider<TDomainObject>(this, principalSecurityPath);
    }

    public ISecurityProvider<TDomainObject> GetBusinessRoleSecurityProvider<TDomainObject>(
            Expression<Func<TDomainObject, BusinessRole>> businessRoleSecurityPath)
            where TDomainObject : PersistentDomainObjectBase
    {
        if (businessRoleSecurityPath == null) throw new ArgumentNullException(nameof(businessRoleSecurityPath));

        return new BusinessRoleSecurityProvider<TDomainObject>(this, businessRoleSecurityPath);
    }

    public ISecurityProvider<Operation> GetOperationSecurityProvider()
    {
        return this.operationSecurityProvider;
    }

    /// <inheritdoc />
    public string GetFormattedPermission(Permission permission, string separator = " | ")
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return this.GetPermissionVisualParts(permission).Join(separator);
    }

    private IEnumerable<string> GetPermissionVisualParts(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        yield return $"Role: {permission.Role}";

        yield return $"Period: {permission.Period}";

        foreach (var entityTypeGroup in permission.FilterItems.GroupBy(fi => fi.Entity.EntityType, fi => fi.Entity.EntityId))
        {
            var securityEntities = this.ExternalSource.GetTyped(entityTypeGroup.Key).GetSecurityEntitiesByIdents(entityTypeGroup);

            yield return $"{entityTypeGroup.Key.Name.ToPluralize()}: {securityEntities.Join(", ")}";
        }
    }

    IAuthorizationBLLContext IAuthorizationBLLContextContainer<IAuthorizationBLLContext>.Authorization => this;
}
