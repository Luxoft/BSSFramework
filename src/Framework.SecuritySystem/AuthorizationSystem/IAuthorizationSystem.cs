using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem
{
    bool IsAdmin();

    bool HasAccess(NonContextSecurityOperation securityOperation);

    void CheckAccess(NonContextSecurityOperation securityOperation);

    string ResolveSecurityTypeName(Type type);
}

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetAccessors(
        NonContextSecurityOperation securityOperation,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        ContextSecurityOperation securityOperation,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(ContextSecurityOperation securityOperation);

    TIdent ResolveSecurityTypeId(Type type);
}
