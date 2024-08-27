using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class AuthorizationSystemExtensions
{
    public static bool IsSecurityAdministrator(this IAuthorizationSystem authorizationSystem) =>
        authorizationSystem.HasAccess(AuthorizationSecurityRule.SecurityAdministrator);
}
