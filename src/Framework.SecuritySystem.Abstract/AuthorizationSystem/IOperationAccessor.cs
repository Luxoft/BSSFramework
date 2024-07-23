namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
