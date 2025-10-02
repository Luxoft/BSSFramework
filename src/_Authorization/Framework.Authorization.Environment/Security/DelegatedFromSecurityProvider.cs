using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystemImpl;
using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment.Security;

public class DelegatedFromSecurityProvider<TDomainObject>(
    ICurrentPrincipalSource currentPrincipalSource,
    [FromKeyedServices(nameof(Permission.DelegatedFrom))] IRelativeDomainPathInfo<TDomainObject, Permission> toPermissionPathInfo)
    : CurrentPrincipalSecurityProvider<TDomainObject>(
        currentPrincipalSource,
        toPermissionPathInfo.Select(permission => permission.Principal));
