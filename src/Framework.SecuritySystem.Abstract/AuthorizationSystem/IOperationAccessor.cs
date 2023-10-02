namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdmin();

    bool HasAccess(SecurityOperation securityOperation);

    void CheckAccess(SecurityOperation securityOperation);
}
