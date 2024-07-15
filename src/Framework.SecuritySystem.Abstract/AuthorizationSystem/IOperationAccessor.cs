namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    bool HasAccess(SecurityRule.RoleBaseSecurityRule securityRule);

    void CheckAccess(SecurityRule.RoleBaseSecurityRule securityRule);
}
