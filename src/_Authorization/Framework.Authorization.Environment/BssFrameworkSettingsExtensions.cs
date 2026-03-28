using CommonFramework.DependencyInjection;

using SecuritySystem.DependencyInjection;

namespace Framework.Authorization.Environment;

public static class BssFrameworkSettingsExtensions
{
    public static ISecuritySystemBuilder AddAuthorizationSystem(this ISecuritySystemBuilder securitySystemBuilder, Action<IAuthorizationSystemBuilder>? setupAction = null) =>
        securitySystemBuilder.Initialize<ISecuritySystemBuilder, AuthorizationSystemBuilder>(setupAction);
}
