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
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        bindingInfo.GeRestrictionsExpr<TSecurityContext>();

    public Expression<Func<TDomainObject, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.GetManyGrandAccessExpr<TSecurityContext>().BuildOr();

    public Expression<Func<TDomainObject, bool>> GetContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.GetManyContainsIdentsExpr<TSecurityContext>(idents).BuildOr();

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

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule).GetPermissionQuery().Any();

    private IEnumerable<Expression<Func<TDomainObject, bool>>> GetManyGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>

        bindingInfo.RestrictionPaths.Select(restrictionPath => restrictionPath switch
        {
            Expression<Func<TDomainObject, TSecurityContext>> singlePath => singlePath.Select(
                securityContext => securityContext == null),

            Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath => manyPath.Select(
                securityContexts => !securityContexts.Any()),

            _ => throw new InvalidOperationException("invalid path")
        });

    private IEnumerable<Expression<Func<TDomainObject, bool>>> GetManyContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>

        bindingInfo.RestrictionPaths.Select(restrictionPath => restrictionPath switch
        {
            Expression<Func<TDomainObject, TSecurityContext>> singlePath => singlePath.Select(
                securityContext => idents.Contains(securityContext.Id)),

            Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath => manyPath.Select(
                securityContexts => securityContexts.Any(securityContext => idents.Contains(securityContext.Id))),

            _ => throw new InvalidOperationException("invalid path")
        });

    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule);
}
