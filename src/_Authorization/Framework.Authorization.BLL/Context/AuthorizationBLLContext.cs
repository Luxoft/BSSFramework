using CommonFramework.DictionaryCache;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;

using Framework.QueryLanguage;
using SecuritySystem;

using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystemImpl;
using Framework.Authorization.SecuritySystemImpl.Validation;
using Framework.Events;
using SecuritySystem.Services;

using Microsoft.Extensions.DependencyInjection;
using SecuritySystem.AvailableSecurity;
using SecuritySystem.ExternalSystem.SecurityContextStorage;
using SecuritySystem.HierarchicalExpand;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices("BLL")] IEventOperationSender operationSender,
    ITrackingService<PersistentDomainObjectBase> trackingService,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IStandartExpressionBuilder standartExpressionBuilder,
    IAuthorizationValidator validator,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
    TimeProvider timeProvider,
    IRootSecurityService<PersistentDomainObjectBase> securityService,
    IAuthorizationBLLFactoryContainer logics,
    ISecurityContextStorage securityContextStorage,
    INotificationPrincipalExtractor notificationPrincipalExtractor,
    ISecuritySystem securitySystem,
    IRunAsManager runAsManager,
    IAvailablePermissionSource availablePermissionSource,
    IAvailableSecurityRoleSource availableSecurityRoleSource,
    ICurrentPrincipalSource currentPrincipalSource,
    IPrincipalGeneralValidator principalValidator,
    ICurrentUser currentUser,
    ISecurityContextInfoSource securityContextInfoSource,
    BLLContextSettings<PersistentDomainObjectBase> settings,
    IAvailableSecurityOperationSource availableSecurityOperationSource)
    : SecurityBLLBaseContext<PersistentDomainObjectBase, Guid, IAuthorizationBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        trackingService,
        accessDeniedExceptionService,
        standartExpressionBuilder,
        validator,
        hierarchicalObjectExpanderFactory,
        fetchService)
{
    private readonly IDictionaryCache<Type, SecurityContextType> securityContextTypeCache = new DictionaryCache<Type, SecurityContextType>(
        securityContextType => logics.SecurityContextType.GetById(
            securityContextInfoSource.GetSecurityContextInfo(securityContextType).Id,
            true)).WithLock();

    public ITypeResolver<string> TypeResolver { get; } = settings.TypeResolver;

    public ISecurityContextInfoSource SecurityContextInfoSource { get; } = securityContextInfoSource;

    public INotificationPrincipalExtractor NotificationPrincipalExtractor { get; } = notificationPrincipalExtractor;

    public ISecuritySystem SecuritySystem { get; } = securitySystem;

    public IPrincipalGeneralValidator PrincipalValidator { get; } = principalValidator;

    public ICurrentPrincipalSource CurrentPrincipalSource { get; } = currentPrincipalSource;

    public ICurrentUser CurrentUser { get; } = currentUser;

    public IRunAsManager RunAsManager { get; } = runAsManager;

    public IAvailablePermissionSource AvailablePermissionSource { get; } = availablePermissionSource;

    public IAvailableSecurityRoleSource AvailableSecurityRoleSource { get; } = availableSecurityRoleSource;

    public IAvailableSecurityOperationSource AvailableSecurityOperationSource { get; } = availableSecurityOperationSource;

    public IRootSecurityService<PersistentDomainObjectBase> SecurityService { get; } = securityService;

    public override IAuthorizationBLLFactoryContainer Logics { get; } = logics;

    public ISecurityContextStorage SecurityContextStorage { get; } = securityContextStorage;

    public TimeProvider TimeProvider { get; } = timeProvider;

    public SecurityContextType GetSecurityContextType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.securityContextTypeCache[type];
    }

    IAuthorizationBLLContext IAuthorizationBLLContextContainer<IAuthorizationBLLContext>.Authorization => this;
}
