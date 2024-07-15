using System.Linq.Expressions;

using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetNonContextAccessors(
        SecurityRule.RoleBaseSecurityRule securityRule,
        Expression<Func<IPermission<TIdent>, bool>> permissionFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        SecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(SecurityRule.RoleBaseSecurityRule securityRule);
}
