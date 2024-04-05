using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetNonContextAccessors(
        SecurityOperation securityRule,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        SecurityOperation securityRule,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(SecurityOperation securityRule);
}
