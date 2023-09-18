namespace Framework.SecuritySystem.DiTests;

public interface IPrincipalPermissionSource<TIdent>
{
    List<Dictionary<Type, List<TIdent>>> GetPermissions();

    IQueryable<IPermission<TIdent>> GetPermissionQuery(ContextSecurityOperation securityOperation);
}
