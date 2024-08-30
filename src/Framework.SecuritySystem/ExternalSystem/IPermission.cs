namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermission
{
    IEnumerable<Guid> GetRestrictions(Type securityContextType);

    string PrincipalName { get; }
}
