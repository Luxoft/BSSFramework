using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

using NHibernate.Linq;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    ICurrentUser currentUser,
    IQueryableSource queryableSource,
    VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermissionSystem<TDomainObject>
{
    public Type PermissionType { get; } = typeof(TDomainObject);

    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        return bindingInfo.GetPermissionRestrictionsExpr<TSecurityContext>();
    }

    public Expression<Func<TDomainObject, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        return bindingInfo.GetGrandAccessExpr<TSecurityContext>();
    }

    public IPermissionSource<TDomainObject> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (securityRuleExpander.FullExpand(securityRule).SecurityRoles.Contains(bindingInfo.SecurityRole))
        {
            return new VirtualPermissionSource<TDomainObject>(currentUser, queryableSource, bindingInfo);
        }
        else
        {
            return new EmptyPermissionSource<TDomainObject>();
        }
    }

    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) =>
        await this.GetPermissionSource(bindingInfo.SecurityRole).GetPermissionQuery().AnyAsync(cancellationToken)
            ? [bindingInfo.SecurityRole]
            : [];

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.GetPermissionSource(securityRule).GetPermissionQuery().Any();
    }

    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.GetPermissionSource(securityRule);
    }
}
