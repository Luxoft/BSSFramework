using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore;

public static class AuthorizationSystemExtensions
{
    public static bool IsSecurityAdministrator(this IAuthorizationSystem authorizationSystem) =>
        authorizationSystem.HasAccess(ApplicationSecurityRule.SecurityAdministrator);
}
