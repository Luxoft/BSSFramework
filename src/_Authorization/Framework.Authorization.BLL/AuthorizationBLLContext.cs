using CommonFramework.Auth;
using CommonFramework.DictionaryCache;

using Framework.Application.Events;
using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Core.TypeResolving;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using HierarchicalExpand;

using SecuritySystem;
using SecuritySystem.Services;
using SecuritySystem.AvailableSecurity;
using SecuritySystem.ExternalSystem.SecurityContextStorage;
using SecuritySystem.AccessDenied;
using SecuritySystem.GeneralPermission.Validation.Principal;
using SecuritySystem.Notification;
using SecuritySystem.UserSource;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices("BLL")] IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IAuthorizationValidator validator,
    TimeProvider timeProvider,
    IRootSecurityService securityService,
    IAuthorizationBLLFactoryContainer logics,
    ISecurityContextStorage securityContextStorage,
    INotificationPrincipalExtractor<Principal> notificationPrincipalExtractor,
    ISecuritySystem securitySystem,
    IRunAsManager runAsManager,
    IAvailablePermissionSource<Permission> availablePermissionSource,
    IAvailableSecurityRoleSource availableSecurityRoleSource,
    ICurrentUserSource<Principal> currentPrincipalSource,
    [FromKeyedServices("Root")] IPrincipalValidator<Principal, Permission, PermissionRestriction> principalValidator,
    ICurrentUser currentUser,
    ISecurityContextInfoSource securityContextInfoSource,
    BLLContextSettings<PersistentDomainObjectBase> settings,
    IAvailableSecurityOperationSource availableSecurityOperationSource)
    : SecurityBLLBaseContext<PersistentDomainObjectBase, Guid, IAuthorizationBLLFactoryContainer>(
        serviceProvider,
        operationSender,
        accessDeniedExceptionService,
        hierarchicalObjectExpanderFactory)
{
    private readonly IDictionaryCache<Type, SecurityContextType> securityContextTypeCache = new DictionaryCache<Type, SecurityContextType>(
        securityContextType => logics.SecurityContextType.GetById(
            (Guid)securityContextInfoSource.GetSecurityContextInfo(securityContextType).Identity.GetId(),
            true)!).WithLock();

    public ITypeResolver<string> TypeResolver { get; } = settings.TypeResolver;

    public ISecurityContextInfoSource SecurityContextInfoSource { get; } = securityContextInfoSource;

    public INotificationPrincipalExtractor<Principal> NotificationPrincipalExtractor { get; } = notificationPrincipalExtractor;

    public ISecuritySystem SecuritySystem { get; } = securitySystem;

    public IValidator Validator { get; } = validator;

    public IPrincipalValidator<Principal, Permission, PermissionRestriction> PrincipalValidator { get; } = principalValidator;

    public ICurrentUserSource<Principal> CurrentPrincipalSource { get; } = currentPrincipalSource;

    public ICurrentUser CurrentUser { get; } = currentUser;

    public IRunAsManager RunAsManager { get; } = runAsManager;

    public IAvailablePermissionSource<Permission> AvailablePermissionSource { get; } = availablePermissionSource;

    public IAvailableSecurityRoleSource AvailableSecurityRoleSource { get; } = availableSecurityRoleSource;

    public IAvailableSecurityOperationSource AvailableSecurityOperationSource { get; } = availableSecurityOperationSource;

    public IRootSecurityService SecurityService { get; } = securityService;

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
