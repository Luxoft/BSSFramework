using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore;

public static class SecuritySystemExtensions
{
    public static bool IsSecurityAdministrator(this ISecuritySystemBase securitySystem) =>
        securitySystem.HasAccess(ApplicationSecurityRule.SecurityAdministrator);
}
