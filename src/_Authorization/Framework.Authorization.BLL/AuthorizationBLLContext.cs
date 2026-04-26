using Anch.Core.Auth;
using Anch.Core.DictionaryCache;

using Framework.Application.Events;
using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.BLL.Services;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using Anch.HierarchicalExpand;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Services;
using Anch.SecuritySystem.ExternalSystem.SecurityContextStorage;
using Anch.SecuritySystem.AccessDenied;
using Anch.SecuritySystem.GeneralPermission.Validation.Principal;
using Anch.SecuritySystem.UserSource;

namespace Framework.Authorization.BLL;

public partial class AuthorizationBLLContext(
    IServiceProvider serviceProvider,
    [FromKeyedServices(nameof(BLL))] IEventOperationSender operationSender,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    IAuthorizationValidator validator,
    IRootSecurityService securityService,
    IAuthorizationBLLFactoryContainer logics,
    ISecurityContextStorage securityContextStorage,
    ISecuritySystem securitySystem,
    IRunAsManager runAsManager,
    ICurrentUserSource<Principal> currentPrincipalSource,
    IPrincipalValidator<Principal, Permission, PermissionRestriction> principalValidator,
    ICurrentUser currentUser,
    ISecurityContextInfoSource securityContextInfoSource)
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

    public ISecurityContextInfoSource SecurityContextInfoSource { get; } = securityContextInfoSource;

    public ISecuritySystem SecuritySystem { get; } = securitySystem;

    public IValidator Validator { get; } = validator;

    public IPrincipalValidator<Principal, Permission, PermissionRestriction> PrincipalValidator { get; } = principalValidator;

    public ICurrentUserSource<Principal> CurrentPrincipalSource { get; } = currentPrincipalSource;

    public ICurrentUser CurrentUser { get; } = currentUser;

    public IRunAsManager RunAsManager { get; } = runAsManager;

    public IRootSecurityService SecurityService { get; } = securityService;

    public override IAuthorizationBLLFactoryContainer Logics { get; } = logics;

    public ISecurityContextStorage SecurityContextStorage { get; } = securityContextStorage;

    public SecurityContextType GetSecurityContextType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.securityContextTypeCache[type];
    }

    IAuthorizationBLLContext IAuthorizationBLLContextContainer<IAuthorizationBLLContext>.Authorization => this;
}
