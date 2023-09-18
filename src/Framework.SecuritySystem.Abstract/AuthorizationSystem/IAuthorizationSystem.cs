namespace Framework.SecuritySystem;

public interface IAuthorizationSystem
{
    public string CurrentPrincipalName { get; }

    bool IsAdmin();

    bool HasAccess(NonContextSecurityOperation securityOperation);

    void CheckAccess(NonContextSecurityOperation securityOperation);
}
