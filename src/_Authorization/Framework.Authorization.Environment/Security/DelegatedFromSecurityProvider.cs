using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment.Security;

public class DelegatedFromSecurityProvider<TDomainObject>(
    ICurrentPrincipalSource currentPrincipalSource,
    [FromKeyedServices(nameof(Permission.DelegatedFrom))] IRelativeDomainPathInfo<TDomainObject, Permission> toPermissionPathInfo)
    : CurrentPrincipalSecurityProvider<TDomainObject>(
        currentPrincipalSource,
        toPermissionPathInfo.Select(permission => permission.Principal));
