namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdministrator() => this.HasAccess(SpecialRoleSecurityRule.Administrator);

    bool HasAccess(SecurityRule.DomainObjectSecurityRule securityRule);

    void CheckAccess(SecurityRule.DomainObjectSecurityRule securityRule);
}
