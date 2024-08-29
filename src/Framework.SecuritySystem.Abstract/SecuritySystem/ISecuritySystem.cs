namespace Framework.SecuritySystem;

public interface ISecuritySystem : ISecuritySystemBase
{
    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
