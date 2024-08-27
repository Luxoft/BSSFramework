using System.Linq.Expressions;

using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    IEnumerable<string> GetAccessors(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        Expression<Func<IPermission<TIdent>, bool>> permissionFilter);
}
