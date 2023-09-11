using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem
{
    bool IsAdmin();

    /// <summary>
    /// NonContext checking
    /// </summary>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    /// <param name="securityOperationCode"></param>
    /// <returns></returns>
    bool HasAccess<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode)
        where TSecurityOperationCode : struct, Enum;

    /// <summary>
    /// NonContext checking
    /// </summary>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    /// <param name="securityOperationCode"></param>
    void CheckAccess<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode)
        where TSecurityOperationCode : struct, Enum;

    string ResolveSecurityTypeName(Type type);
}

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetAccessors<TSecurityOperationCode>(
        TSecurityOperationCode securityOperationCode,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter)
        where TSecurityOperationCode : struct, Enum;

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions<TSecurityOperationCode>(
        ContextSecurityOperation<TSecurityOperationCode> securityOperation,
        IEnumerable<Type> securityTypes)
        where TSecurityOperationCode : struct, Enum;

    IQueryable<IPermission<TIdent>> GetPermissionQuery<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
        where TSecurityOperationCode : struct, Enum;

    TIdent ResolveSecurityTypeId(Type type);
}
