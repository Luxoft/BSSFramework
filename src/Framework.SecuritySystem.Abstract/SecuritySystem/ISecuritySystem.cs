namespace Framework.SecuritySystem;

public interface ISecuritySystem
{
    bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
