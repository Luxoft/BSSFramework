using SecuritySystem.DependencyInjection;

namespace Framework.Authorization.Environment;

public static class BssFrameworkSettingsExtensions
{
    public static ISecuritySystemSettings AddAuthorizationSystem(
        this ISecuritySystemSettings settings,
        Action<IAuthorizationSystemSettings>? setup = null)
    {
        var authSettings = new AuthorizationSystemSettings();

        setup?.Invoke(authSettings);

        authSettings.Initialize(settings);

        return settings;
    }
}
