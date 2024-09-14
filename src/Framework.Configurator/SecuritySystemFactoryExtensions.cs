using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;

namespace Framework.Configurator;

internal static class SecuritySystemFactoryExtensions
{
    public static bool IsAdministrator(this ISecuritySystemFactory securitySystemFactory) =>
        securitySystemFactory.Create(false).IsAdministrator();

    public static bool IsSecurityAdministrator(this ISecuritySystemFactory securitySystemFactory) =>
        securitySystemFactory.HasAccess(ApplicationSecurityRule.SecurityAdministrator);

    public static bool HasAccess(this ISecuritySystemFactory securitySystemFactory, DomainSecurityRule.RoleBaseSecurityRule securityRule) =>
        securitySystemFactory.Create(false).HasAccess(securityRule);

    public static void CheckAccess(
        this ISecuritySystemFactory securitySystemFactory,
        DomainSecurityRule.RoleBaseSecurityRule securityRule) =>

        securitySystemFactory.Create(false).CheckAccess(securityRule);
}
