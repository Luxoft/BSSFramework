namespace Framework.SecuritySystem;

public interface ISecuritySystem
{
    bool HasAccess(DomainSecurityRule securityRule);

    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    void CheckAccess(DomainSecurityRule securityRule);
}
