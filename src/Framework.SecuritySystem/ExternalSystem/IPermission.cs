namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermission
{
    IEnumerable<IPermissionRestriction> Restrictions { get; }

    string PrincipalName { get; }
}
