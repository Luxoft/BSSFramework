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

public class VirtualPermissionSystem<TDomainObject> : IPermissionSystem<TDomainObject>
{
    private readonly ISecurityRuleExpander securityRuleExpander;

    private readonly ICurrentUser currentUser;

    private readonly IQueryableSource queryableSource;

    private readonly VirtualPermissionBindingInfo<TDomainObject> bindingInfo;

    public VirtualPermissionSystem(
        ISecurityRuleExpander securityRuleExpander,
        ICurrentUser currentUser,
        IQueryableSource queryableSource,
        ISecurityRoleSource securityRoleSource,
        VirtualPermissionBindingInfo<TDomainObject> bindingInfo)
    {
        this.securityRuleExpander = securityRuleExpander;
        this.currentUser = currentUser;
        this.queryableSource = queryableSource;
        this.bindingInfo = bindingInfo;

        this.bindingInfo.Validate(securityRoleSource);
    }

    public Type PermissionType { get; } = typeof(TDomainObject);

    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.bindingInfo.GetRestrictionsExpr<TSecurityContext>();

    public Expression<Func<TDomainObject, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.GetManyGrandAccessExpr<TSecurityContext>().BuildOr();

    public Expression<Func<TDomainObject, bool>> GetContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.GetManyContainsIdentsExpr<TSecurityContext>(idents).BuildOr();

    public IPermissionSource<TDomainObject> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (this.securityRuleExpander.FullExpand(securityRule).SecurityRoles.Contains(this.bindingInfo.SecurityRole))
        {
            return new VirtualPermissionSource<TDomainObject>(this.currentUser, this.queryableSource, this.bindingInfo);
        }
        else
        {
            return new EmptyPermissionSource<TDomainObject>();
        }
    }

    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) =>
        await this.GetPermissionSource(this.bindingInfo.SecurityRole).GetPermissionQuery().AnyAsync(cancellationToken)
            ? [this.bindingInfo.SecurityRole]
            : [];

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule).GetPermissionQuery().Any();

    private IEnumerable<Expression<Func<TDomainObject, bool>>> GetManyGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var restrictionPath in this.bindingInfo.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TDomainObject, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(
                    securityContext => securityContext == null);
            }
            else if (restrictionPath is Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => !securityContexts.Any());
            }
        }
    }

    private IEnumerable<Expression<Func<TDomainObject, bool>>> GetManyContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var restrictionPath in this.bindingInfo.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TDomainObject, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(
                    securityContext => idents.Contains(securityContext.Id));
            }
            else if (restrictionPath is Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => securityContexts.Any(securityContext => idents.Contains(securityContext.Id)));
            }
        }
    }

    IPermissionSource IPermissionSystem.GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule);
}
