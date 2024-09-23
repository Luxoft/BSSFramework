using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem.ExternalSystem.Management;

namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionSystem
{
    Type PermissionType { get; }

    IPrincipalService PrincipalService { get; }

    IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default);
}

public interface IPermissionSystem<TPermission> : IPermissionSystem
{
    Expression<Func<TPermission, IEnumerable<Guid>>> GetPermissionRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext;

    Expression<Func<TPermission, bool>> GetGrandAccessExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext =>

        this.GetPermissionRestrictionsExpr<TSecurityContext>().Select(v => !v.Any());

    Expression<Func<TPermission, bool>> GetContainsIdentsExpr<TSecurityContext>(IEnumerable<Guid> idents)
        where TSecurityContext : ISecurityContext =>
        this.GetPermissionRestrictionsExpr<TSecurityContext>().Select(restrictionIdents => restrictionIdents.Any(restrictionIdent => idents.Contains(restrictionIdent)));

    new IPermissionSource<TPermission> GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
