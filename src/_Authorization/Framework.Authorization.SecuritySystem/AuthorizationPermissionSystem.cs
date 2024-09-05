using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSystem(
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IAvailablePermissionSource availablePermissionSource,
    ISecurityContextSource securityContextSource,
    IServiceProvider serviceProvider)
    : AuthorizationSystemBase(accessDeniedExceptionService, availablePermissionSource, true), IPermissionSystem<Permission>
{
    public Type PermissionType { get; } = typeof(Permission);

    public Expression<Func<Permission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        var securityContextTypeId = securityContextSource.GetSecurityContextInfo<TSecurityContext>().Id;

        return permission => permission.Restrictions
                                       .Where(restriction => restriction.SecurityContextType.Id == securityContextTypeId)
                                       .Select(restriction => restriction.SecurityContextId);
    }

    public IPermissionSource<Permission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationPermissionSource>(serviceProvider, securityRule);
    }

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationAvailableSecurityRoleSource>(serviceProvider)
                                 .GetAvailableSecurityRoles(cancellationToken);
    }
    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.GetPermissionSource(securityRule);
    }
}
