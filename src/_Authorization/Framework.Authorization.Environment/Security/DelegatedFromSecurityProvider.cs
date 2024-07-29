using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment.Security;

public class DelegatedFromSecurityProvider<TDomainObject>(
    IActualPrincipalSource actualPrincipalSource,
    [FromKeyedServices(nameof(Permission.DelegatedFrom))] IRelativeDomainPathInfo<TDomainObject, Permission> toPermissionPathInfo)
    : PrincipalSecurityProvider<TDomainObject>(
        actualPrincipalSource,
        toPermissionPathInfo.Select(permission => permission.Principal))

    where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>;
