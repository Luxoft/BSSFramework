using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetNonContextAccessors(
        SecurityOperation securityOperation,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        SecurityOperation securityOperation,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(SecurityOperation securityOperation);
}
