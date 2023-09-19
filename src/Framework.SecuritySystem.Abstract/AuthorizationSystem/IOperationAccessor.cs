namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdmin();

    bool HasAccess(NonContextSecurityOperation securityOperation);

    void CheckAccess(NonContextSecurityOperation securityOperation);
}
