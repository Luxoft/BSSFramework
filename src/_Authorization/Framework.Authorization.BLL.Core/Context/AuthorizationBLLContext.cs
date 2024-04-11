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
    private readonly IDictionaryCache<string, SecurityContextType> securityContextTypeByNameCache;

    private readonly IDictionaryCache<Guid, SecurityContextType> securityContextTypeByIdCache;

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
        this.AvailableSecurityRoleSource = availableSecurityRoleSource;
        this.ActualPrincipalSource = actualPrincipalSource;
        this.NotificationPrincipalExtractor = notificationPrincipalExtractor;
        this.AuthorizationSystem = authorizationSystem;
        this.RunAsManager = runAsManager;

        this.ExternalSource = externalSource ?? throw new ArgumentNullException(nameof(externalSource));

        this.lazyCurrentPrincipal = LazyHelper.Create(() => this.Logics.Principal.GetCurrent());

        this.securityContextTypeByNameCache = new DictionaryCache<string, SecurityContextType>(
                                                                             domainTypeName => this.Logics.SecurityContextType.GetByName(domainTypeName, true),
                                                                             StringComparer.CurrentCultureIgnoreCase)
                .WithLock();

        this.securityContextTypeByIdCache = new DictionaryCache<Guid, SecurityContextType>(
                                                                         domainTypeId => this.Logics.SecurityContextType.GetById(domainTypeId, true))
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

    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; }

    public override IAuthorizationBLLFactoryContainer Logics => this.logics;

    public IAuthorizationExternalSource ExternalSource { get; }

    public Principal CurrentPrincipal => this.lazyCurrentPrincipal.Value;


    public TimeProvider TimeProvider { get; }


    public SecurityContextType GetSecurityContextType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.GetSecurityContextType(type.Name);
    }

    public SecurityContextType GetSecurityContextType(string domainTypeName)
    {
        if (domainTypeName == null) throw new ArgumentNullException(nameof(domainTypeName));

        return this.securityContextTypeByNameCache[domainTypeName];
    }

    public SecurityContextType GetSecurityContextType(Guid domainTypeId)
    {
        if (domainTypeId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainTypeId));

        return this.securityContextTypeByIdCache[domainTypeId];
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

        foreach (var securityContextTypeGroup in permission.Restrictions.GroupBy(fi => fi.SecurityContextType, fi => fi.SecurityContextId))
        {
            var securityEntities = this.ExternalSource.GetTyped(securityContextTypeGroup.Key).GetSecurityEntitiesByIdents(securityContextTypeGroup);

            yield return $"{securityContextTypeGroup.Key.Name.ToPluralize()}: {securityEntities.Join(", ")}";
        }
    }

    IAuthorizationBLLContext IAuthorizationBLLContextContainer<IAuthorizationBLLContext>.Authorization => this;
}
