using Framework.Authorization.Domain;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment.Security;

public class DelegatedFromSecurityProvider<TDomainObject>(
    ICurrentUser currentUser,
    [FromKeyedServices(nameof(Permission.DelegatedFrom))] IRelativeDomainPathInfo<TDomainObject, Permission> toPermissionPathInfo)
    : PrincipalSecurityProvider<TDomainObject>(
        currentUser,
        toPermissionPathInfo.Select(permission => permission.Principal))

    where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>;
