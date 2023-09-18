namespace Framework.SecuritySystem;

public interface IAuthorizationSystem
{
    bool IsAdmin();

    bool HasAccess(NonContextSecurityOperation securityOperation);

    void CheckAccess(NonContextSecurityOperation securityOperation);
}
