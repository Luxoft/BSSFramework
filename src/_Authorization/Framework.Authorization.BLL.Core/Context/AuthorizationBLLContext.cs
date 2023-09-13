using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.Tracking;
using Framework.HierarchicalExpand;
using Framework.Projection;
using Framework.QueryLanguage;
using Framework.SecuritySystem;


using Framework.Authorization.Notification;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext
{
    private readonly IAuthorizationBLLFactoryContainer logics;

    private readonly IRuntimePermissionOptimizationService optimizeRuntimePermissionService;

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
            IUserAuthenticationService userAuthenticationService,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            IConfigurationBLLContext configuration,
            IAuthorizationSecurityService securityService,
            IAuthorizationBLLFactoryContainer logics,
            IAuthorizationExternalSource externalSource,
            IRunAsManager runAsManager,
            ISecurityTypeResolverContainer securityTypeResolverContainer,
            IRuntimePermissionOptimizationService optimizeRuntimePermissionService,
            INotificationPrincipalExtractor notificationPrincipalExtractor,
            IAuthorizationBLLContextSettings settings)
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
        this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
        this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        this.logics = logics ?? throw new ArgumentNullException(nameof(logics));
        this.optimizeRuntimePermissionService = optimizeRuntimePermissionService ?? throw new ArgumentNullException(nameof(optimizeRuntimePermissionService));
        this.NotificationPrincipalExtractor = notificationPrincipalExtractor;

        this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));
        this.RunAsManager = runAsManager ?? throw new ArgumentNullException(nameof(runAsManager));
        this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        this.lazyCurrentPrincipal = LazyHelper.Create(() => this.Logics.Principal.GetCurrent());

        this.CurrentPrincipalName = userAuthenticationService.GetUserName();

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

    public IRunAsManager RunAsManager { get; }

    public INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    public string CurrentPrincipalName { get; }

    public IAuthorizationSecurityService SecurityService { get; }

    public Settings Settings => this.lazySettings.Value;

    public override IAuthorizationBLLFactoryContainer Logics => this.logics;

    public IAuthorizationExternalSource ExternalSource { get; }

    public Principal CurrentPrincipal => this.lazyCurrentPrincipal.Value;


    public IDateTimeService DateTimeService { get; }

    public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }

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

    private bool HasAccess(AvailablePermissionFilter filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return this.Logics.Permission.GetSecureQueryable().Where(filter.ToFilterExpression()).Any();
    }

    public bool HasAccess(Operation operation)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        return this.HasAccess(new AvailablePermissionOperationFilter(this.DateTimeService, this.RunAsManager.PrincipalName, operation));
    }

    public bool HasAccess(NonContextSecurityOperation securityOperation, bool withRunAs)
    {
        var typedSecurityOperation = (NonContextSecurityOperation<Guid>)securityOperation;

        var principalName = withRunAs ? this.RunAsManager.PrincipalName : this.CurrentPrincipalName;

        var filter = new AvailablePermissionOperationFilter(
            this.DateTimeService,
            principalName,
            typedSecurityOperation.Id);

        return this.HasAccess(filter);
    }

    public bool IsAdmin()
    {
        return this.Logics.BusinessRole.HasAdminRole();
    }

    public bool HasAccess(NonContextSecurityOperation securityOperation)
    {
        return this.HasAccess(securityOperation, true);
    }

    public void CheckAccess(NonContextSecurityOperation securityOperation)
    {
        this.CheckAccess(securityOperation, true);
    }

    public void CheckAccess(NonContextSecurityOperation securityOperation, bool withRunAs)
    {
        if (!this.HasAccess(securityOperation, withRunAs))
        {
            throw this.AccessDeniedExceptionService.GetAccessDeniedException(
                new AccessResult.AccessDeniedResult
                {
                    SecurityOperation = securityOperation
                });
        }
    }

    public Guid ResolveSecurityTypeId(Type type)
    {
        return this.GetEntityType(type.GetProjectionSourceTypeOrSelf()).Id;
    }

    public string ResolveSecurityTypeName(Type type)
    {
        return type.GetProjectionSourceTypeOrSelf().Name;
    }

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
            ContextSecurityOperation securityOperation,
            IEnumerable<Type> securityTypes)
    {
        if (securityTypes == null) throw new ArgumentNullException(nameof(securityTypes));

        var typedSecurityOperation = (ContextSecurityOperation<Guid>)securityOperation;

        var filter = new AvailablePermissionOperationFilter(
            this.DateTimeService,
            this.RunAsManager.PrincipalName,
            typedSecurityOperation.Id);

        var permissions = this.Logics.Permission.GetListBy(
                                                           filter, z => z.SelectMany(q => q.FilterItems).SelectNested(q => q.Entity).Select(q => q.EntityType));

        var securityTypesCache = securityTypes.ToReadOnlyCollection();

        return permissions
               .Select(permission => permission.ToDictionary(securityTypesCache))
               .Pipe(this.optimizeRuntimePermissionService.Optimize)
               .ToList(permission => this.TryExpandPermission(permission, securityOperation.ExpandType));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(ContextSecurityOperation securityOperation)
    {

        var typedSecurityOperation = (ContextSecurityOperation<Guid>)securityOperation;

        var filter = new AvailablePermissionOperationFilter(
            this.DateTimeService,
            this.RunAsManager.PrincipalName,
            typedSecurityOperation.Id);

        return this.Logics.Permission.GetUnsecureQueryable().Where(filter.ToFilterExpression());
    }

    private IEnumerable<string> GetAccessors(Expression<Func<Principal, bool>> principalFilter, AvailablePermissionFilter permissionFilter)
    {
        if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));
        if (permissionFilter == null) throw new ArgumentNullException(nameof(permissionFilter));

        var extraPrincipalFilter =
                principalFilter.UpdateBody(
                                           new OverridePropertyInfoVisitor<Principal, IEnumerable<Permission>>(
                                            principal => principal.Permissions, permissionFilter.ToFilterExpression().ToCollectionFilter()));

        var principals = this.Logics.Principal.GetListBy(extraPrincipalFilter);

        return principals.Select(principal => principal.Name);
    }

    public IEnumerable<string> GetAccessors(Operation operation, Expression<Func<Principal, bool>> principalFilter)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));

        return this.GetAccessors(principalFilter, new AvailablePermissionOperationFilter(this.DateTimeService, null, operation));
    }

    //public Guid GrandAccessIdent { get; } = DenormalizedPermissionItem.GrandAccessGuid;

    public IEnumerable<string> GetAccessors(
            NonContextSecurityOperation securityOperation, Expression<Func<IPrincipal<Guid>, bool>> principalFilter)
    {
        if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));

        var typedSecurityOperation = (NonContextSecurityOperation<Guid>)securityOperation;

        return this.GetAccessors(
                                 (Expression<Func<Principal, bool>>)AuthVisitor.Visit(principalFilter),
                                 new AvailablePermissionOperationFilter(this.DateTimeService, null, typedSecurityOperation.Id));
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(Dictionary<Type, List<Guid>> permission, HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
                                       pair => pair.Key,
                                       pair => this.HierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
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

    private static readonly ExpressionVisitor AuthVisitor = new OverrideParameterTypeVisitor(
     new Dictionary<Type, Type>
     {
             { typeof(IPrincipal<Guid>), typeof(Principal) },
             { typeof(IPermission<Guid>), typeof(Permission) },
             { typeof(IPermissionFilterItem<Guid>), typeof(PermissionFilterItem) },
             { typeof(IPermissionFilterEntity<Guid>), typeof(PermissionFilterEntity) },
             { typeof(IEntityType<Guid>), typeof(EntityType) },
     });
}
