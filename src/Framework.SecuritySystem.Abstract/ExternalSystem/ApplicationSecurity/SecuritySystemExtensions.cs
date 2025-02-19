namespace Framework.SecuritySystem.ExternalSystem.ApplicationSecurity;

public static class SecuritySystemExtensions
{
    public static bool IsSecurityAdministrator(this ISecuritySystem securitySystem) =>
        securitySystem.HasAccess(ApplicationSecurityRule.SecurityAdministrator);
}
