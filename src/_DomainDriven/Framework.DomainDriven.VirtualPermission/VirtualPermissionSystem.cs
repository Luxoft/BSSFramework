using System.Linq.Expressions;

using Framework.Core;
using Framework.GenericQueryable;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TPrincipal, TPermission>(
    IServiceProvider serviceProvider,
    ISecurityRuleExpander securityRuleExpander,
    IUserNameResolver userNameResolver,
    IQueryableSource queryableSource,
    TimeProvider timeProvider,
    SecurityRuleCredential securityRuleCredential,
    VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo)
    : IPermissionSystem<TPermission>

    where TPrincipal : IIdentityObject<Guid>
    where TPermission : class, IIdentityObject<Guid>
{
    public Type PermissionType { get; } = typeof(TPermission);

    public Expression<Func<TPermission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>(
        SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext =>
        bindingInfo.GetRestrictionsExpr(restrictionFilterInfo?.GetPureFilter(serviceProvider));

    public Expression<Func<TPermission, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : class, ISecurityContext =>
        this.GetManyGrandAccessExpr<TSecurityContext>().BuildAnd();

    public Expression<Func<TPermission, bool>> GetContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents, SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext =>
        this.GetManyContainsIdentsExpr(idents, restrictionFilterInfo).BuildOr();

    public IPermissionSource<TPermission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (securityRuleExpander.FullRoleExpand(securityRule).SecurityRoles.Contains(bindingInfo.SecurityRole))
        {
            return new VirtualPermissionSource<TPrincipal, TPermission>(
                serviceProvider,
                userNameResolver,
                queryableSource,
                timeProvider,
                bindingInfo,
                securityRule,
                securityRuleCredential);
        }
        else
        {
            return new EmptyPermissionSource<TPermission>();
        }
    }

    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) =>
        await this.GetPermissionSource(bindingInfo.SecurityRole).GetPermissionQuery().GenericAnyAsync(cancellationToken)
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

    private IEnumerable<Expression<Func<TPermission, bool>>> GetManyContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents, SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
        where TSecurityContext : ISecurityContext
    {
        foreach (var restrictionPath in bindingInfo.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TPermission, TSecurityContext>> singlePath)
            {
                if (restrictionFilterInfo == null)
                {
                    yield return singlePath.Select(securityContext => idents.Contains(securityContext.Id));
                }
                else
                {
                    var securityContextFilter = restrictionFilterInfo.GetPureFilter(serviceProvider)
                                                                     .BuildAnd(securityContext => idents.Contains(securityContext.Id));

                    yield return singlePath.Select(securityContextFilter);
                }
            }
            else if (restrictionPath is Expression<Func<TPermission, IEnumerable<TSecurityContext>>> manyPath)
            {
                if (restrictionFilterInfo == null)
                {
                    yield return manyPath.Select(securityContexts => securityContexts.Any(securityContext => idents.Contains(securityContext.Id)));
                }
                else
                {
                    var securityContextFilter = restrictionFilterInfo.GetPureFilter(serviceProvider).ToEnumerableAny()
                                                                     .BuildAnd(securityContexts => securityContexts.Any(securityContext => idents.Contains(securityContext.Id)));

                    yield return manyPath.Select(securityContextFilter);
                }
            }
        }
    }

    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule);
}
