namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdmin();

    bool HasAccess(SecurityRule.DomainObjectSecurityRule securityRule);

    void CheckAccess(SecurityRule.DomainObjectSecurityRule securityRule);
}
