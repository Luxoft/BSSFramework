namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionSystem : IAuthorizationSystem
{
    IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
