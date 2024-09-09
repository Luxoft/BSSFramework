using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.ExternalSystem.Management;
using Framework.SecuritySystem.UserSource;

using NHibernate.Linq;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TPrincipal, TPermission> : IPermissionSystem<TPermission>
    where TPrincipal : IIdentityObject<Guid>
    where TPermission : IIdentityObject<Guid>
{
    private readonly ISecurityRuleExpander securityRuleExpander;

    private readonly ICurrentUser currentUser;

    private readonly IQueryableSource queryableSource;

    private readonly TimeProvider timeProvider;

    private readonly VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo;

    public VirtualPermissionSystem(
        ISecurityRuleExpander securityRuleExpander,
        ICurrentUser currentUser,
        IQueryableSource queryableSource,
        TimeProvider timeProvider,
        ISecurityRoleSource securityRoleSource,
        VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo)
    {
        this.securityRuleExpander = securityRuleExpander;
        this.currentUser = currentUser;
        this.queryableSource = queryableSource;
        this.timeProvider = timeProvider;
        this.bindingInfo = bindingInfo;

        this.bindingInfo.Validate(securityRoleSource);

        this.PrincipalService = new VirtualPrincipalService<TPrincipal, TPermission>(queryableSource, bindingInfo);
    }

    public Type PermissionType { get; } = typeof(TPermission);

    public IPrincipalService PrincipalService { get; }

    public Expression<Func<TPermission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.bindingInfo.GetRestrictionsExpr<TSecurityContext>();

    public Expression<Func<TPermission, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.GetManyGrandAccessExpr<TSecurityContext>().BuildOr();

    public Expression<Func<TPermission, bool>> GetContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.GetManyContainsIdentsExpr<TSecurityContext>(idents).BuildOr();

    public IPermissionSource<TPermission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (this.securityRuleExpander.FullExpand(securityRule).SecurityRoles.Contains(this.bindingInfo.SecurityRole))
        {
            return new VirtualPermissionSource<TPrincipal, TPermission>(this.currentUser, this.queryableSource, this.timeProvider, this.bindingInfo);
        }
        else
        {
            return new EmptyPermissionSource<TPermission>();
        }
    }

    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) =>
        await this.GetPermissionSource(this.bindingInfo.SecurityRole).GetPermissionQuery().AnyAsync(cancellationToken)
            ? [this.bindingInfo.SecurityRole]
            : [];

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => this.GetPermissionSource(securityRule).GetPermissionQuery().Any();

    private IEnumerable<Expression<Func<TPermission, bool>>> GetManyGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var restrictionPath in this.bindingInfo.RestrictionPaths)
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
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var restrictionPath in this.bindingInfo.RestrictionPaths)
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
