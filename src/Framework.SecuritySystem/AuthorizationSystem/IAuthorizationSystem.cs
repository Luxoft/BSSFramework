using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetNonContextAccessors(
        SecurityRule securityRule,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        SecurityRule securityRule,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(SecurityRule securityRule);
}
