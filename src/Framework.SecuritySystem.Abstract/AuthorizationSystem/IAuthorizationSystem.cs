namespace Framework.SecuritySystem;

public interface IAuthorizationSystem
{
    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
