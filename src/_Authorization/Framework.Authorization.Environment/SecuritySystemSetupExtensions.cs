using CommonFramework.DependencyInjection;

using SecuritySystem.DependencyInjection;

namespace Framework.Authorization.Environment;

public static class SecuritySystemSetupExtensions
{
    public static ISecuritySystemSetup AddAuthorizationSystem(this ISecuritySystemSetup securitySystemSetup, Action<IAuthorizationSystemSetup>? setupAction = null) =>
        securitySystemSetup.Initialize<ISecuritySystemSetup, AuthorizationSystemSetup>(setupAction);
}
