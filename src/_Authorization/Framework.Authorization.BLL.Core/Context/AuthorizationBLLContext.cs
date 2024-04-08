using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;

using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.SecuritySystem;

using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext
{
    private readonly IAuthorizationBLLFactoryContainer logics;

    private readonly Lazy<Principal> lazyCurrentPrincipal;
    private readonly IDictionaryCache<string, EntityType> entityTypeByNameCache;

    private readonly IDictionaryCache<Guid, EntityType> entityTypeByIdCache;

    public AuthorizationBLLContext(
            IServiceProvider serviceProvider,
            [FromKeyedServices("BLL")] IEventOperationSender operationSender,
            ITrackingService<PersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            IAuthorizationValidator validator,
            IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            TimeProvider timeProvider,
            IRootSecurityService<PersistentDomainObjectBase> securityService,
            IAuthorizationBLLFactoryContainer logics,
            IAuthorizationExternalSource externalSource,
            INotificationPrincipalExtractor notificationPrincipalExtractor,
            IAuthorizationBLLContextSettings settings,
            IAuthorizationSystem<Guid> authorizationSystem,
            IRunAsManager runAsManager,
            IAvailablePermissionSource availablePermissionSource,
            ISecurityOperationParser<Guid> securityOperationParser,
            IAvailableSecurityRoleSource availableSecurityRoleSource,
            IActualPrincipalSource actualPrincipalSource)
            : base(
                   serviceProvider,
                   operationSender,
                   trackingService,
                   accessDeniedExceptionService,
                   standartExpressionBuilder,
                   validator,
                   hierarchicalObjectExpanderFactory,
                   fetchService)
    {
        this.TimeProvider = timeProvider;
        this.SecurityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        this.logics = logics ?? throw new ArgumentNullException(nameof(logics));
        this.AvailablePermissionSource = availablePermissionSource;
        this.SecurityOperationParser = securityOperationParser;
        this.AvailableSecurityRoleSource = availableSecurityRoleSource;
        this.ActualPrincipalSource = actualPrincipalSource;
        this.NotificationPrincipalExtractor = notificationPrincipalExtractor;
        this.AuthorizationSystem = authorizationSystem;
        this.RunAsManager = runAsManager;

        this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));

        this.lazyCurrentPrincipal = LazyHelper.Create(() => this.Logics.Principal.GetCurrent());

        this.entityTypeByNameCache = new DictionaryCache<string, EntityType>(
                                                                             domainTypeName => this.Logics.EntityType.GetByName(domainTypeName, true),
                                                                             StringComparer.CurrentCultureIgnoreCase)
                .WithLock();

        this.entityTypeByIdCache = new DictionaryCache<Guid, EntityType>(
                                                                         domainTypeId => this.Logics.EntityType.GetById(domainTypeId, true))
                .WithLock();

        this.TypeResolver = settings.TypeResolver;
    }

    public ITypeResolver<string> TypeResolver { get; }

    public INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    public IAuthorizationSystem<Guid> AuthorizationSystem { get; }

    public IActualPrincipalSource ActualPrincipalSource { get; }

    public IRunAsManager RunAsManager { get; }

    public IAvailablePermissionSource AvailablePermissionSource { get; }

    public IAvailableSecurityRoleSource AvailableSecurityRoleSource { get; }

    public ISecurityOperationParser<Guid> SecurityOperationParser { get; }

    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; }

    public override IAuthorizationBLLFactoryContainer Logics => this.logics;

    public IAuthorizationExternalSource ExternalSource { get; }

    public Principal CurrentPrincipal => this.lazyCurrentPrincipal.Value;


    public TimeProvider TimeProvider { get; }


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
