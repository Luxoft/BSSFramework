namespace Framework.SecuritySystem;

public interface ISecuritySystemBase
{
    bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
