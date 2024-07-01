namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermission<out TIdent>
{
    IEnumerable<IPermissionRestriction<TIdent>> Restrictions { get; }

    string PrincipalName { get; }
}
