using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSystem(
    IServiceProvider serviceProvider,
    ISecurityContextSource securityContextSource)
    : IPermissionSystem<Permission>
{
    public Type PermissionType { get; } = typeof(Permission);

    public IPrincipalService PrincipalService { get; } = ActivatorUtilities.CreateInstance<AuthorizationPrincipalService>(serviceProvider);

    public Expression<Func<Permission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        var securityContextTypeId = securityContextSource.GetSecurityContextInfo<TSecurityContext>().Id;

        return permission => permission.Restrictions
                                       .Where(restriction => restriction.SecurityContextType.Id == securityContextTypeId)
                                       .Select(restriction => restriction.SecurityContextId);
    }

    public IPermissionSource<Permission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule, bool withRunAs)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationPermissionSource>(serviceProvider, securityRule, withRunAs);
    }

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationAvailableSecurityRoleSource>(serviceProvider)
                                 .GetAvailableSecurityRoles(cancellationToken);
    }
    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule, bool withRunAs)
    {
        return this.GetPermissionSource(securityRule, withRunAs);
    }
}
