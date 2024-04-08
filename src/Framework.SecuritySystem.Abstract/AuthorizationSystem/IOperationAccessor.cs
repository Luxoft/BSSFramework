namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdmin();

    bool HasAccess(SecurityRule securityRule);

    void CheckAccess(SecurityRule securityRule);
}
