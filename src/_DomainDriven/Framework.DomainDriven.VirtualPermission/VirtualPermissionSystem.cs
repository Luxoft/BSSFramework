using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;

using NHibernate.Linq;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TPrincipal, TPermission>(
    ISecurityRuleExpander securityRuleExpander,
    IUserNameResolver userNameResolver,
    IQueryableSource queryableSource,
    TimeProvider timeProvider,
    SecurityRuleCredential securityRuleCredential,
    VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo)
    : IPermissionSystem<TPermission>

    where TPrincipal : IIdentityObject<Guid>
    where TPermission : IIdentityObject<Guid>
{
    public Type PermissionType { get; } = typeof(TPermission);
    
    public Expression<Func<TPermission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext =>
        bindingInfo.GetRestrictionsExpr<TSecurityContext>();

    public Expression<Func<TPermission, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext =>
        this.GetManyGrandAccessExpr<TSecurityContext>().BuildOr();

    public Expression<Func<TPermission, bool>> GetContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext =>
        this.GetManyContainsIdentsExpr<TSecurityContext>(idents).BuildOr();

    public IPermissionSource<TPermission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (securityRuleExpander.FullExpand(securityRule).SecurityRoles.Contains(bindingInfo.SecurityRole))
        {
            return new VirtualPermissionSource<TPrincipal, TPermission>(
                userNameResolver,
                queryableSource,
                timeProvider,
                bindingInfo,
                securityRuleCredential);
        }
        else
        {
            return new EmptyPermissionSource<TPermission>();
        }
    }

    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) =>
        await this.GetPermissionSource(bindingInfo.SecurityRole).GetPermissionQuery().AnyAsync(cancellationToken)
            ? [bindingInfo.SecurityRole]
            : [];

    private IEnumerable<Expression<Func<TPermission, bool>>> GetManyGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext
    {
        foreach (var restrictionPath in bindingInfo.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TPermission, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(
                    securityContext => securityContext == null);
            }
            else if (restrictionPath is Expression<Func<TPermission, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => !securityContexts.Any());
            }
        }
    }

    private IEnumerable<Expression<Func<TPermission, bool>>> GetManyContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext
    {
        foreach (var restrictionPath in bindingInfo.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TPermission, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(
                    securityContext => idents.Contains(securityContext.Id));
            }
            else if (restrictionPath is Expression<Func<TPermission, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => securityContexts.Any(securityContext => idents.Contains(securityContext.Id)));
            }
        }
    }

    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule);
}
