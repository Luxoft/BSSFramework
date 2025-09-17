using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSystem(
    IServiceProvider serviceProvider,
    ISecurityContextInfoSource securityContextInfoSource,
    ISecurityContextSource securityContextSource,
    SecurityRuleCredential securityRuleCredential)
    : IPermissionSystem<Permission>
{
    public Type PermissionType { get; } = typeof(Permission);

    public Expression<Func<Permission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>(SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext
    {
        var securityContextTypeId = securityContextInfoSource.GetSecurityContextInfo<TSecurityContext>().Id;

        if (restrictionFilterInfo == null)
        {
            return permission => permission.Restrictions
                                           .Where(restriction => restriction.SecurityContextType.Id == securityContextTypeId)
                                           .Select(restriction => restriction.SecurityContextId);
        }
        else
        {
            var securityContextQueryable = securityContextSource.GetQueryable(restrictionFilterInfo)
                                                                .Where(restrictionFilterInfo.GetPureFilter(serviceProvider))
                                                                .Select(securityContext => securityContext.Id);

            return permission => permission.Restrictions
                                           .Where(restriction => restriction.SecurityContextType.Id == securityContextTypeId)
                                           .Where(restriction => securityContextQueryable.Contains(restriction.SecurityContextId))
                                           .Select(restriction => restriction.SecurityContextId);
        }
    }

    public IPermissionSource<Permission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationPermissionSource>(serviceProvider, securityRule.TryApplyCredential(securityRuleCredential));
    }

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationAvailableSecurityRoleSource>(serviceProvider, securityRuleCredential)
                                 .GetAvailableSecurityRoles(cancellationToken);
    }
    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.GetPermissionSource(securityRule);
    }
}
